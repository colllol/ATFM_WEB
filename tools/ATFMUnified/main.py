from __future__ import annotations

import queue
import sys
import threading
import traceback
import tkinter as tk
from pathlib import Path
from tkinter import messagebox, scrolledtext, ttk


TOOLS_ROOT = Path(__file__).resolve().parents[1]
if str(TOOLS_ROOT) not in sys.path:
    sys.path.insert(0, str(TOOLS_ROOT))

from TracksSyncPython.main import MessageSink, execute  # noqa: E402
from excel_panel import ExcelImporterPanel  # noqa: E402


class TracksLoggerPanel(ttk.Frame):
    """Tracks Logger độc lập, giao tiếp với Tk chỉ qua hàng đợi sự kiện."""

    def __init__(self, parent: tk.Misc) -> None:
        super().__init__(parent, padding=14)
        self.stop_event = threading.Event()
        self.events: queue.Queue[tuple[str, object]] = queue.Queue()
        self.busy = False
        self.auto_running = False
        self._build_ui()
        self.after(100, self._poll_events)

    def _build_ui(self) -> None:
        self.columnconfigure(0, weight=1)
        self.rowconfigure(4, weight=1)

        ttk.Label(self, text="FIR TRACKS LOGGER", font=("Segoe UI", 13, "bold")).grid(
            row=0, column=0, sticky="w"
        )
        ttk.Label(
            self,
            text="public.tracks → FIR VVHN/VVHM → T_DAY_FLIGHTS_GOINGON → T_TRACKS_LOG",
            foreground="#526b7a",
        ).grid(row=1, column=0, sticky="w", pady=(2, 12))

        controls = ttk.Frame(self)
        controls.grid(row=2, column=0, sticky="ew")
        self.manual_buttons = [
            ttk.Button(controls, text="Kiểm tra đối chiếu", command=lambda: self._start("check")),
            ttk.Button(controls, text="Ghi T_TRACKS_LOG", command=lambda: self._start("sync")),
            ttk.Button(controls, text="Xác minh kết quả", command=lambda: self._start("verify")),
        ]
        for button in self.manual_buttons:
            button.pack(side="left", padx=(0, 7), pady=3)

        self.auto_button = ttk.Button(controls, text="Auto", command=self._start_auto)
        self.auto_button.pack(side="left", padx=(5, 7), pady=3)
        ttk.Label(controls, text="Chu kỳ (giây):").pack(side="left", padx=(3, 5))
        self.interval_value = tk.StringVar(value="10")
        self.interval_entry = ttk.Entry(controls, textvariable=self.interval_value, width=8, justify="center")
        self.interval_entry.pack(side="left", ipady=3)

        self.status_value = tk.StringVar(value="Sẵn sàng")
        ttk.Label(self, textvariable=self.status_value, foreground="#47677d").grid(
            row=3, column=0, sticky="ew", pady=(8, 3)
        )
        self.log = scrolledtext.ScrolledText(
            self,
            bg="#102531",
            fg="#e8f7ff",
            insertbackground="white",
            font=("Consolas", 9),
            wrap="word",
            relief="flat",
        )
        self.log.grid(row=4, column=0, sticky="nsew")

    def _set_controls(self, enabled: bool) -> None:
        state = "normal" if enabled else "disabled"
        for button in self.manual_buttons:
            button.configure(state=state)
        if not self.auto_running:
            self.auto_button.configure(state=state)
            self.interval_entry.configure(state=state)

    def _append_log(self, text: str) -> None:
        self.log.insert("end", text + "\n")
        self.log.see("end")

    def _start(self, mode: str) -> None:
        if self.busy or self.auto_running:
            return
        self.busy = True
        self._set_controls(False)
        self.status_value.set("Đang xử lý...")

        panel = self

        class QueueSink(MessageSink):
            def write(self, text: str) -> None:
                panel.events.put(("log", text))

        def worker() -> None:
            try:
                panel.events.put(("log", "Đang chạy, vui lòng chờ..."))
                execute(mode, full=False, sink=QueueSink())
            except Exception:
                panel.events.put(("log", traceback.format_exc()))
                panel.events.put(("error", "Tác vụ Tracks Logger gặp lỗi. Xem nhật ký để biết chi tiết."))
            finally:
                panel.events.put(("manual_done", None))

        threading.Thread(target=worker, daemon=True, name=f"tracks-{mode}").start()

    def _start_auto(self) -> None:
        if self.busy or self.auto_running:
            return
        try:
            interval = int(self.interval_value.get().strip())
            if interval <= 0:
                raise ValueError
        except ValueError:
            messagebox.showwarning("Chu kỳ không hợp lệ", "Vui lòng nhập số giây là số nguyên lớn hơn 0.")
            self.interval_entry.focus_set()
            return

        self.busy = True
        self.auto_running = True
        self._set_controls(False)
        self.auto_button.configure(text="Auto đang chạy", state="disabled")
        self.interval_entry.configure(state="disabled")
        panel = self

        class QueueSink(MessageSink):
            def write(self, text: str) -> None:
                panel.events.put(("log", text))

        def worker() -> None:
            iteration = 0
            while not panel.stop_event.is_set():
                iteration += 1
                panel.events.put(("log", f"\n========== AUTO - LƯỢT {iteration} =========="))
                panel.events.put(("status", f"Đang chạy lượt {iteration}..."))
                try:
                    execute("all", full=False, sink=QueueSink())
                    panel.events.put(("log", f"Auto lượt {iteration} đã hoàn thành."))
                except Exception:
                    panel.events.put(("log", f"Auto lượt {iteration} gặp lỗi:\n{traceback.format_exc()}"))

                for remaining in range(interval, 0, -1):
                    if panel.stop_event.is_set():
                        return
                    panel.events.put(("status", f"Lượt {iteration} hoàn tất · chạy lại sau {remaining} giây"))
                    if panel.stop_event.wait(1):
                        return

        threading.Thread(target=worker, daemon=True, name="tracks-auto").start()

    def _poll_events(self) -> None:
        try:
            while True:
                event, value = self.events.get_nowait()
                if event == "log":
                    self._append_log(str(value))
                elif event == "status":
                    self.status_value.set(str(value))
                elif event == "error":
                    messagebox.showerror("Tracks Logger", str(value))
                elif event == "manual_done":
                    self.busy = False
                    self.status_value.set("Sẵn sàng")
                    self._set_controls(True)
        except queue.Empty:
            pass
        if not self.stop_event.is_set():
            self.after(100, self._poll_events)

    def shutdown(self) -> None:
        self.stop_event.set()


class ToolShell(tk.Frame):
    def __init__(self, parent: tk.Misc, title: str, collapse_text: str, collapse_command) -> None:
        super().__init__(parent, bg="#d6e3ed", highlightthickness=1, highlightbackground="#bfd0dd")
        self.rowconfigure(1, weight=1)
        self.columnconfigure(0, weight=1)
        title_bar = tk.Frame(self, bg="#eaf3f9")
        title_bar.grid(row=0, column=0, sticky="ew")
        title_bar.columnconfigure(0, weight=1)
        tk.Label(
            title_bar,
            text=title,
            bg="#eaf3f9",
            fg="#174f75",
            font=("Segoe UI", 11, "bold"),
            padx=12,
            pady=8,
        ).grid(row=0, column=0, sticky="w")
        tk.Button(
            title_bar,
            text=collapse_text,
            command=collapse_command,
            bg="#1f79ad",
            fg="white",
            activebackground="#155b83",
            activeforeground="white",
            relief="flat",
            cursor="hand2",
            font=("Segoe UI", 9, "bold"),
            padx=10,
            pady=5,
        ).grid(row=0, column=1, padx=7, pady=5)

    def set_content(self, widget: tk.Widget) -> None:
        widget.grid(row=1, column=0, sticky="nsew")


class UnifiedATFMApp(tk.Tk):
    def __init__(self) -> None:
        super().__init__()
        self.title("ATFM - Data Tools Center")
        self.geometry("1600x900")
        self.minsize(1100, 680)
        self.configure(bg="#eef3f7")
        self.protocol("WM_DELETE_WINDOW", self._close)
        self.layout_state = "both"
        self._build_ui()

    def _build_ui(self) -> None:
        self.columnconfigure(0, weight=1)
        self.rowconfigure(1, weight=1)

        header = tk.Frame(self, bg="#164d73", height=72)
        header.grid(row=0, column=0, sticky="ew")
        header.grid_propagate(False)
        header.columnconfigure(0, weight=1)
        tk.Label(
            header,
            text="ATFM · DATA TOOLS CENTER",
            bg="#164d73",
            fg="white",
            font=("Segoe UI", 20, "bold"),
        ).grid(row=0, column=0, sticky="w", padx=24, pady=(12, 0))
        tk.Label(
            header,
            text="Excel Importer và FIR Tracks Logger hoạt động độc lập",
            bg="#164d73",
            fg="#bfe4fa",
            font=("Segoe UI", 9),
        ).grid(row=1, column=0, sticky="w", padx=25, pady=(0, 10))

        self.workspace = tk.Frame(self, bg="#eef3f7")
        self.workspace.grid(row=1, column=0, sticky="nsew", padx=10, pady=10)
        self.workspace.rowconfigure(0, weight=1)

        self.left_shell = ToolShell(
            self.workspace, "EXCEL IMPORTER · KHH / SLOT", "◀  Thu gọn", self._collapse_left
        )
        self.excel_panel = ExcelImporterPanel(self.left_shell)
        self.left_shell.set_content(self.excel_panel)

        self.right_shell = ToolShell(
            self.workspace, "FIR TRACKS LOGGER", "Thu gọn  ▶", self._collapse_right
        )
        self.tracks_panel = TracksLoggerPanel(self.right_shell)
        self.right_shell.set_content(self.tracks_panel)

        self.left_rail = self._create_rail("▶", "Mở Excel Importer", self._expand_both)
        self.right_rail = self._create_rail("◀", "Mở FIR Tracks Logger", self._expand_both)
        self._apply_layout()

    def _create_rail(self, arrow: str, label: str, command) -> tk.Frame:
        rail = tk.Frame(self.workspace, bg="#dceaf3", width=58, highlightthickness=1, highlightbackground="#bdd0dd")
        rail.grid_propagate(False)
        rail.rowconfigure(0, weight=1)
        rail.columnconfigure(0, weight=1)
        rail_label = label.replace(" ", "\n")
        button = tk.Button(
            rail,
            text=f"{arrow}\n\n{rail_label}",
            command=command,
            bg="#1f79ad",
            fg="white",
            activebackground="#155b83",
            activeforeground="white",
            relief="flat",
            cursor="hand2",
            font=("Segoe UI", 8, "bold"),
            wraplength=48,
        )
        button.grid(row=0, column=0, sticky="nsew", padx=5, pady=5)
        return rail

    def _collapse_left(self) -> None:
        self.layout_state = "left_collapsed"
        self._apply_layout()

    def _collapse_right(self) -> None:
        self.layout_state = "right_collapsed"
        self._apply_layout()

    def _expand_both(self) -> None:
        self.layout_state = "both"
        self._apply_layout()

    def _apply_layout(self) -> None:
        for widget in (self.left_shell, self.right_shell, self.left_rail, self.right_rail):
            widget.grid_remove()
        for column in (0, 1):
            # Phải xóa uniform của trạng thái chia đôi. Nếu giữ lại, cột đã
            # thu gọn vẫn chiếm nửa cửa sổ và panel còn lại không thể mở rộng.
            self.workspace.columnconfigure(column, weight=0, minsize=0, uniform="")

        if self.layout_state == "left_collapsed":
            self.workspace.columnconfigure(0, weight=0, minsize=58)
            self.workspace.columnconfigure(1, weight=1)
            self.left_rail.grid(row=0, column=0, sticky="ns", padx=(0, 6))
            self.right_shell.grid(row=0, column=1, sticky="nsew")
        elif self.layout_state == "right_collapsed":
            self.workspace.columnconfigure(0, weight=1)
            self.workspace.columnconfigure(1, weight=0, minsize=58)
            self.left_shell.grid(row=0, column=0, sticky="nsew")
            self.right_rail.grid(row=0, column=1, sticky="ns", padx=(6, 0))
        else:
            self.workspace.columnconfigure(0, weight=1, uniform="tools")
            self.workspace.columnconfigure(1, weight=1, uniform="tools")
            self.left_shell.grid(row=0, column=0, sticky="nsew", padx=(0, 5))
            self.right_shell.grid(row=0, column=1, sticky="nsew", padx=(5, 0))

    def _close(self) -> None:
        self.excel_panel.shutdown()
        self.tracks_panel.shutdown()
        self.destroy()


def main() -> None:
    UnifiedATFMApp().mainloop()


if __name__ == "__main__":
    main()
