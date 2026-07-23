from __future__ import annotations

import os
import queue
import threading
import tkinter as tk
from pathlib import Path
from tkinter import filedialog, messagebox, ttk

from khh_importer import parse_khh_file, supports_folder, _target_date_from_path
from oracle_db import OracleDatabase
from scanner import scan_folders
from slot_importer import parse_slot_file, supports_slot_folder, flight_date_from_root


class ExcelImporterPanel(ttk.Frame):
    """Giao diện Excel Importer nhúng được, giữ nguyên luồng xử lý nghiệp vụ."""

    def __init__(self, parent: tk.Misc) -> None:
        super().__init__(parent)
        self.events: queue.Queue[tuple[str, object]] = queue.Queue()
        self.folder_items: dict[str, tuple[Path, list[Path]]] = {}
        self.file_items: dict[Path, str] = {}
        self.import_order: list[Path] = []
        self.cancel_event = threading.Event()
        self._build_ui()
        self.after(100, self._poll_events)

    def _build_ui(self) -> None:
        self.columnconfigure(0, weight=1)
        self.rowconfigure(3, weight=1)
        style = ttk.Style(self)
        if "clam" in style.theme_names():
            style.theme_use("clam")
        style.configure("TFrame", background="#ffffff")
        style.configure("Card.TFrame", background="#ffffff")
        style.configure("TLabel", background="#ffffff", foreground="#27364b", font=("Segoe UI", 9))
        style.configure("Title.TLabel", background="#ffffff", foreground="#172b4d", font=("Segoe UI", 11, "bold"))
        style.configure("Hint.TLabel", background="#ffffff", foreground="#718096", font=("Segoe UI", 8))
        style.configure("TEntry", fieldbackground="#f8fafc", bordercolor="#d8e1eb", padding=7)
        style.configure("TButton", font=("Segoe UI", 9), padding=(11, 7), background="#edf2f7", borderwidth=0)
        style.map("TButton", background=[("active", "#e2e8f0")])
        style.configure("Accent.TButton", font=("Segoe UI", 10, "bold"), foreground="white", background="#2563eb", padding=(18, 10), borderwidth=0)
        style.map("Accent.TButton", background=[("active", "#1d4ed8"), ("disabled", "#9cb7e9")], foreground=[("disabled", "#eef4ff")])
        style.configure("Danger.TButton", foreground="#b42318", background="#fff1f0")
        style.configure("Treeview", rowheight=29, font=("Segoe UI", 9), background="#ffffff", fieldbackground="#ffffff", foreground="#344054", borderwidth=0)
        style.map("Treeview", background=[("selected", "#e8f1ff")], foreground=[("selected", "#175cd3")])
        style.configure("Treeview.Heading", font=("Segoe UI", 9, "bold"), foreground="#475467", background="#f8fafc", relief="flat", padding=(6, 8))
        style.configure("Modern.Horizontal.TProgressbar", troughcolor="#e5eaf1", background="#22a06b", borderwidth=0, thickness=8)

        header = tk.Frame(self, bg="#102a43", height=76)
        header.grid(row=0, column=0, sticky="ew")
        header.columnconfigure(0, weight=1)
        title_box = tk.Frame(header, bg="#102a43")
        title_box.grid(row=0, column=0, sticky="w", padx=24, pady=14)
        tk.Label(title_box, text="ATFM  /  DATA OPERATIONS", bg="#102a43", fg="#8ec5ff", font=("Segoe UI", 8, "bold")).pack(anchor="w")
        tk.Label(title_box, text="Excel Import Center", bg="#102a43", fg="white", font=("Segoe UI Semibold", 20)).pack(anchor="w")
        self.header_status = tk.Label(header, text="●  SẴN SÀNG", bg="#173f5f", fg="#7ee2b8", font=("Segoe UI", 9, "bold"), padx=14, pady=7)
        self.header_status.grid(row=0, column=1, padx=24)

        source_frame = ttk.Frame(self, style="Card.TFrame", padding=14)
        source_frame.grid(row=1, column=0, sticky="ew", padx=18, pady=(16, 8))
        source_frame.columnconfigure(0, weight=1)
        ttk.Label(source_frame, text="THƯ MỤC NGUỒN", style="Title.TLabel").grid(row=0, column=0, sticky="w", columnspan=3)
        ttk.Label(source_frame, text="Chọn thư mục ngày chứa KHH, SLOT CHK và các nhóm dữ liệu.", style="Hint.TLabel").grid(row=1, column=0, sticky="w", columnspan=3, pady=(1, 9))
        self.root_path = tk.StringVar(value=str(Path.cwd() / "NGAY 0107"))
        ttk.Entry(source_frame, textvariable=self.root_path).grid(row=2, column=0, sticky="ew", padx=(0, 8))
        ttk.Button(source_frame, text="📁  Chọn thư mục", command=self._choose_folder).grid(row=2, column=1, padx=4)
        self.check_button = ttk.Button(source_frame, text="↻  Kiểm tra", command=self._scan)
        self.check_button.grid(row=2, column=2, padx=(4, 0))

        connection = ttk.Frame(self, style="Card.TFrame", padding=(14, 10))
        connection.grid(row=2, column=0, sticky="ew", padx=18, pady=8)
        ttk.Label(connection, text="KẾT NỐI ORACLE", style="Title.TLabel").grid(row=0, column=0, sticky="w", columnspan=10, pady=(0, 7))
        for index in (1, 3, 5, 7, 9):
            connection.columnconfigure(index, weight=1)
        self.host = tk.StringVar(value="172.29.187.90")
        self.port = tk.StringVar(value="1521")
        self.service = tk.StringVar(value="PDBORCL")
        self.user = tk.StringVar(value="ATFM")
        self.password = tk.StringVar(value=os.environ.get("ATFM_DB_PASSWORD", ""))
        fields = (("Host", self.host), ("Port", self.port), ("Service", self.service), ("User", self.user), ("Password", self.password))
        for index, (label, variable) in enumerate(fields):
            field = ttk.Frame(connection, style="Card.TFrame")
            field.grid(row=1, column=index * 2, columnspan=2, sticky="ew", padx=(0, 12 if index < 4 else 0))
            field.columnconfigure(0, weight=1)
            ttk.Label(field, text=label.upper(), style="Hint.TLabel").grid(row=0, column=0, sticky="w")
            ttk.Entry(field, textvariable=variable, show="●" if label == "Password" else "", width=14).grid(row=1, column=0, sticky="ew", pady=(3, 0))

        content = ttk.Panedwindow(self, orient="horizontal")
        content.grid(row=3, column=0, sticky="nsew", padx=18, pady=(8, 18))

        explorer_card = ttk.Frame(content, style="Card.TFrame", padding=12)
        explorer_card.rowconfigure(2, weight=1)
        explorer_card.columnconfigure(0, weight=1)
        content.add(explorer_card, weight=3)
        ttk.Label(explorer_card, text="DỮ LIỆU ĐÃ PHÁT HIỆN", style="Title.TLabel").grid(row=0, column=0, sticky="w")
        self.scan_summary = ttk.Label(explorer_card, text="Nhấn Kiểm tra để quét thư mục", style="Hint.TLabel")
        self.scan_summary.grid(row=1, column=0, sticky="w", pady=(1, 8))
        self.tree = ttk.Treeview(explorer_card, columns=("type", "count", "status"), show="tree headings", selectmode="browse")
        self.tree.heading("#0", text="Thư mục / File Excel")
        self.tree.heading("type", text="Loại")
        self.tree.heading("count", text="Số file")
        self.tree.heading("status", text="Trạng thái")
        self.tree.column("#0", width=390)
        self.tree.column("type", width=78, anchor="center")
        self.tree.column("count", width=65, anchor="center")
        self.tree.column("status", width=135, anchor="center")
        scrollbar = ttk.Scrollbar(explorer_card, orient="vertical", command=self.tree.yview)
        self.tree.configure(yscrollcommand=scrollbar.set)
        self.tree.grid(row=2, column=0, sticky="nsew")
        scrollbar.grid(row=2, column=1, sticky="ns")
        self.tree.tag_configure("supported", foreground="#067647")
        self.tree.tag_configure("unsupported", foreground="#98a2b3")
        self.tree.tag_configure("file", foreground="#475467")
        self.tree.tag_configure("working", foreground="#175cd3")
        self.tree.tag_configure("success", foreground="#067647")
        self.tree.tag_configure("warning", foreground="#b54708")
        self.tree.tag_configure("failed", foreground="#b42318")
        self.tree.bind("<Double-1>", self._toggle_selected_folder)
        self.tree.bind("<space>", self._toggle_selected_folder)

        action_card = ttk.Frame(content, style="Card.TFrame", padding=14)
        action_card.rowconfigure(5, weight=1)
        action_card.columnconfigure(0, weight=1)
        content.add(action_card, weight=2)
        ttk.Label(action_card, text="HÀNG ĐỢI IMPORT", style="Title.TLabel").grid(row=0, column=0, sticky="w")
        self.order_summary = ttk.Label(action_card, text="Chưa chọn thư mục • Double-click bên trái để thêm", style="Hint.TLabel")
        self.order_summary.grid(row=1, column=0, columnspan=2, sticky="w", pady=(1, 8))
        self.order_list = tk.Listbox(action_card, width=32, height=5, exportselection=False, relief="flat", bd=0, highlightthickness=1, highlightbackground="#dce3ec", selectbackground="#dbeafe", selectforeground="#175cd3", bg="#f8fafc", fg="#344054", font=("Segoe UI", 10), activestyle="none")
        self.order_list.grid(row=2, column=0, sticky="ew", ipady=4)
        controls = ttk.Frame(action_card, style="Card.TFrame")
        controls.grid(row=2, column=1, sticky="ns", padx=(8, 0))
        ttk.Button(controls, text="↑", width=4, command=lambda: self._move_order(-1)).pack(pady=(0, 4))
        ttk.Button(controls, text="↓", width=4, command=lambda: self._move_order(1)).pack(pady=4)
        ttk.Button(controls, text="✕", width=4, command=self._remove_order, style="Danger.TButton").pack(pady=4)
        ttk.Separator(action_card).grid(row=3, column=0, columnspan=2, sticky="ew", pady=12)
        ttk.Label(action_card, text="NHẬT KÝ HOẠT ĐỘNG", style="Title.TLabel").grid(row=4, column=0, sticky="w")
        self.log = tk.Text(action_card, width=42, height=8, state="disabled", wrap="word", relief="flat", bd=0, padx=10, pady=8, bg="#142334", fg="#d6e4f0", insertbackground="white", font=("Cascadia Mono", 8))
        self.log.grid(row=5, column=0, columnspan=2, sticky="nsew", pady=(7, 10))
        self.progress_label = ttk.Label(action_card, text="Sẵn sàng", style="Hint.TLabel")
        self.progress_label.grid(row=6, column=0, columnspan=2, sticky="w")
        self.progress = ttk.Progressbar(action_card, mode="determinate", style="Modern.Horizontal.TProgressbar")
        self.progress.grid(row=7, column=0, columnspan=2, sticky="ew", pady=(4, 12))
        action_buttons = ttk.Frame(action_card, style="Card.TFrame")
        action_buttons.grid(row=8, column=0, columnspan=2, sticky="ew")
        action_buttons.columnconfigure(0, weight=1)
        self.import_button = ttk.Button(action_buttons, text="BẮT ĐẦU IMPORT", command=self._start_import, style="Accent.TButton")
        self.import_button.grid(row=0, column=0, sticky="ew", padx=(0, 6))
        self.cancel_button = ttk.Button(action_buttons, text="Hủy", command=self.cancel_event.set, state="disabled", style="Danger.TButton")
        self.cancel_button.grid(row=0, column=1, sticky="ew")

    def _choose_folder(self) -> None:
        selected = filedialog.askdirectory(initialdir=self.root_path.get() or str(Path.cwd()))
        if selected:
            self.root_path.set(selected)

    def _scan(self) -> None:
        try:
            folders = scan_folders(Path(self.root_path.get().strip()))
        except Exception as exc:
            messagebox.showerror("Không thể kiểm tra", str(exc))
            return
        self.tree.delete(*self.tree.get_children())
        self.folder_items.clear()
        self.file_items.clear()
        self.import_order.clear()
        self._refresh_order()
        total = 0
        for folder, files in folders:
            supported = supports_folder(folder) or supports_slot_folder(folder)
            status = "Sẵn sàng" if supported else "Chưa hỗ trợ"
            item = self.tree.insert("", "end", text=f"☑  {folder.name}", values=("Thư mục", len(files), status), open=True, tags=("supported" if supported else "unsupported",))
            self.folder_items[item] = (folder, files)
            self.import_order.append(folder)
            total += len(files)
            for path in files:
                relative = path.relative_to(folder)
                file_item = self.tree.insert(item, "end", text=f"   ▫  {relative}", values=("Excel", "", "Chờ import"), tags=("file",))
                self.file_items[path] = file_item
        self._refresh_order()
        supported_count = sum(1 for folder, _files in folders if supports_folder(folder) or supports_slot_folder(folder))
        self.scan_summary.configure(text=f"{len(folders)} thư mục  •  {total} file Excel  •  {supported_count} nhóm sẵn sàng import")
        self._write_log(f"Đã kiểm tra {len(folders)} thư mục, {total} file Excel.")

    def _toggle_selected_folder(self, _event: object = None) -> None:
        item = self.tree.focus()
        if item not in self.folder_items:
            return
        folder, _files = self.folder_items[item]
        if folder in self.import_order:
            self.import_order.remove(folder)
            checked = False
        else:
            self.import_order.append(folder)
            checked = True
        self.tree.item(item, text=f"{'☑' if checked else '☐'}  {folder.name}")
        self._refresh_order()

    def _refresh_order(self) -> None:
        self.order_list.delete(0, tk.END)
        for index, folder in enumerate(self.import_order, 1):
            self.order_list.insert(tk.END, f"  {index:02d}   {folder.name}")
        if self.import_order:
            self.order_summary.configure(text=f"{len(self.import_order)} thư mục • Import tuần tự từ trên xuống")
        else:
            self.order_summary.configure(text="Chưa chọn thư mục • Double-click bên trái để thêm")

    def _move_order(self, delta: int) -> None:
        selection = self.order_list.curselection()
        if not selection:
            return
        old = selection[0]
        new = old + delta
        if new < 0 or new >= len(self.import_order):
            return
        self.import_order[old], self.import_order[new] = self.import_order[new], self.import_order[old]
        self._refresh_order()
        self.order_list.selection_set(new)

    def _remove_order(self) -> None:
        selection = self.order_list.curselection()
        if not selection:
            return
        folder = self.import_order.pop(selection[0])
        for item, (candidate, _files) in self.folder_items.items():
            if candidate == folder:
                self.tree.item(item, text=f"☐  {folder.name}")
                break
        self._refresh_order()

    def _start_import(self) -> None:
        if not self.import_order:
            messagebox.showwarning("Chưa chọn", "Double-click vào thư mục để thêm vào thứ tự import.")
            return
        unsupported = [folder.name for folder in self.import_order if not (supports_folder(folder) or supports_slot_folder(folder))]
        if unsupported:
            messagebox.showerror("Chưa hỗ trợ", "Chưa có bảng đích/mapping cho: " + ", ".join(unsupported))
            return
        try:
            connection_values = self._connection_values()
        except ValueError as exc:
            messagebox.showerror("Thiếu cấu hình", str(exc))
            return
        files_by_folder = {folder: files for folder, files in self.folder_items.values()}
        work = [(folder, files_by_folder[folder]) for folder in self.import_order]
        self.cancel_event.clear()
        self.progress.configure(value=0, maximum=len(work))
        self.progress_label.configure(text=f"Đang chuẩn bị 0/{len(work)} thư mục")
        self.header_status.configure(text="●  ĐANG IMPORT", fg="#ffd666")
        self.import_button.configure(state="disabled")
        self.check_button.configure(state="disabled")
        self.cancel_button.configure(state="normal")
        threading.Thread(target=self._import_worker, args=(work, connection_values), daemon=True).start()

    def _connection_values(self) -> tuple[str, int, str, str, str]:
        password = self.password.get()
        if not password:
            raise ValueError("Chưa nhập password Oracle")
        try:
            port = int(self.port.get())
        except ValueError as exc:
            raise ValueError("Port Oracle không hợp lệ") from exc
        return self.host.get().strip(), port, self.service.get().strip(), self.user.get().strip(), password

    def _import_worker(self, work: list[tuple[Path, list[Path]]], connection_values: tuple[str, int, str, str, str]) -> None:
        database: OracleDatabase | None = None
        imported_files = 0
        skipped_files = 0
        inserted_total = 0
        duplicate_total = 0
        try:
            self.events.put(("log", "Đang kết nối Oracle..."))
            database = OracleDatabase(*connection_values)
            if any(supports_folder(folder) for folder, _files in work):
                database.verify_schema()
            if any(supports_slot_folder(folder) for folder, _files in work):
                database.ensure_slot_table()
            self.events.put(("log", "Kết nối và kiểm tra bảng thành công."))
            for index, (folder, files) in enumerate(work, 1):
                if self.cancel_event.is_set():
                    self.events.put(("log", "Đã hủy trước khi xử lý thư mục tiếp theo."))
                    break
                self.events.put(("folder_status", (folder, "Đang xử lý", "working")))
                self.events.put(("log", f"[{index}/{len(work)}] Bắt đầu {folder.name}: {len(files)} file."))
                folder_ok = 0
                folder_failed = 0
                target_date = _target_date_from_path(folder) if supports_folder(folder) else flight_date_from_root(folder)
                for file_index, path in enumerate(files, 1):
                    if self.cancel_event.is_set():
                        self.events.put(("log", "Đã hủy trước khi xử lý file tiếp theo."))
                        break
                    self.events.put(("file_status", (path, "Đang đọc...", "working")))
                    try:
                        if supports_folder(folder):
                            records = parse_khh_file(path, target_date)
                            importer = database.import_khh
                        else:
                            records = parse_slot_file(path, target_date)
                            importer = database.import_slots
                        if not records:
                            raise ValueError("không tìm thấy chuyến bay/dòng dữ liệu phù hợp")
                        inserted, skipped = importer(records)
                        imported_files += 1
                        folder_ok += 1
                        inserted_total += inserted
                        duplicate_total += skipped
                        if inserted:
                            status = f"Đã import: {inserted} dòng"
                        else:
                            status = f"Đã tồn tại: {skipped} dòng"
                        self.events.put(("file_status", (path, status, "success")))
                        self.events.put(("log", f"  ✓ [{file_index}/{len(files)}] {path.name}: thêm {inserted}, đã có {skipped}."))
                    except Exception as file_error:
                        skipped_files += 1
                        folder_failed += 1
                        self.events.put(("file_status", (path, "Bỏ qua - lỗi dữ liệu", "failed")))
                        self.events.put(("log", f"  ✗ [{file_index}/{len(files)}] {path.name}: {file_error}. Tiếp tục file khác."))
                folder_status = f"Hoàn tất {folder_ok}/{len(files)} file"
                if folder_failed:
                    folder_status += f" • lỗi {folder_failed}"
                self.events.put(("folder_status", (folder, folder_status, "warning" if folder_failed else "success")))
                self.events.put(("progress", index))
            self.events.put(("done", (imported_files, skipped_files, inserted_total, duplicate_total)))
        except Exception as exc:
            self.events.put(("error", str(exc)))
        finally:
            if database:
                database.close()

    def _poll_events(self) -> None:
        try:
            while True:
                event, value = self.events.get_nowait()
                if event == "log":
                    self._write_log(str(value))
                elif event == "progress":
                    self.progress.configure(value=int(value))
                    self.progress_label.configure(text=f"Đã hoàn tất {int(value)}/{int(float(self.progress.cget('maximum')))} thư mục")
                elif event == "file_status":
                    path, status, tag = value
                    item = self.file_items.get(path)
                    if item:
                        current = self.tree.item(item, "values")
                        self.tree.item(item, values=(current[0], current[1], status), tags=(tag,))
                        self.tree.see(item)
                elif event == "folder_status":
                    folder, status, tag = value
                    for item, (candidate, files) in self.folder_items.items():
                        if candidate == folder:
                            self.tree.item(item, values=("Thư mục", len(files), status), tags=(tag,))
                            break
                elif event == "error":
                    self._write_log("LỖI: " + str(value))
                    self._finish_worker()
                    messagebox.showerror("Import thất bại", str(value))
                elif event == "done":
                    self._finish_worker()
                    imported, failed, inserted, duplicates = value
                    messagebox.showinfo(
                        "Hoàn tất",
                        f"Đã xử lý xong.\n\nFile thành công: {imported}\nFile bỏ qua do lỗi: {failed}\nDòng mới: {inserted}\nDòng đã tồn tại: {duplicates}",
                    )
        except queue.Empty:
            pass
        self.after(100, self._poll_events)

    def _finish_worker(self) -> None:
        self.import_button.configure(state="normal")
        self.check_button.configure(state="normal")
        self.cancel_button.configure(state="disabled")
        self.header_status.configure(text="●  SẴN SÀNG", fg="#7ee2b8")
        if float(self.progress.cget("value")) >= float(self.progress.cget("maximum")):
            self.progress_label.configure(text="Hoàn tất tiến trình import")

    def _write_log(self, message: str) -> None:
        self.log.configure(state="normal")
        self.log.insert(tk.END, message + "\n")
        self.log.see(tk.END)
        self.log.configure(state="disabled")

    def shutdown(self) -> None:
        """Yêu cầu worker dừng ở điểm an toàn tiếp theo khi đóng ứng dụng."""
        self.cancel_event.set()
