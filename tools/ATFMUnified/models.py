from __future__ import annotations

from dataclasses import dataclass
from datetime import date


@dataclass(frozen=True)
class FlightRecord:
    flight_date: date
    callsign: str
    origin: str
    destination: str
    etd: str
    eta: str
    operator: str
    source_file: str = ""
    source_row: int = 0

    @property
    def key(self) -> tuple[object, ...]:
        return (
            self.flight_date,
            self.callsign,
            self.origin,
            self.destination,
            self.etd,
            self.eta,
            self.operator,
        )


@dataclass(frozen=True)
class SlotRecord:
    etd_eta: str
    carrier: str
    aircraft: str
    seats: str
    from_airport: str
    to_airport: str
    callsign: str
    flight_date: date
    aero: str
    source_file: str = ""
    source_row: int = 0

    @property
    def key(self) -> tuple[object, ...]:
        return (
            self.etd_eta,
            self.carrier,
            self.aircraft,
            self.seats,
            self.from_airport,
            self.to_airport,
            self.callsign,
            self.flight_date,
            self.aero,
        )

