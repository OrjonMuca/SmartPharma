import { useState } from "react";
import { RX_QUEUE } from "./data.js";

const MEDS = [
  { name: "Concor 5mg",       info: "1 tab/day · 30-day supply · Cardiology",      price: "26 RON", avail: "Available",  color: "#10b981", emoji: "❤️" },
  { name: "Aspenter 75mg",    info: "1 tab/day · 30-day supply · Antiplatelet",     price: "12 RON", avail: "Available",  color: "#10b981", emoji: "💊" },
  { name: "Atorvastatin 20mg",info: "1 tab/night · 30-day supply · Lipid-lowering", price: "34 RON", avail: "Low stock",  color: "#f59e0b", emoji: "🧪" },
];

export default function PrescriptionsPage() {
  const [rxSelected, setRxSelected] = useState(0);
  const patient = RX_QUEUE[rxSelected];

  return (
    <div style={{ display: "flex", flex: 1, overflow: "hidden" }}>
      {/* Rx Queue sidebar */}
      <div style={{ width: 280, background: "#fff", borderRight: "1.5px solid #f1f5f9", padding: 16, overflow: "auto" }}>
        <div style={{ display: "flex", alignItems: "center", gap: 8, marginBottom: 14 }}>
          <h2 style={{ fontWeight: 700, fontSize: 15, color: "#0f172a" }}>Rx Queue</h2>
          <span style={{ background: "#f59e0b", color: "#fff", fontSize: 11, fontWeight: 700, borderRadius: 99, padding: "2px 7px" }}>8</span>
        </div>
        <div style={{ background: "#f8fafc", borderRadius: 8, padding: "8px 10px", marginBottom: 12, display: "flex", alignItems: "center", gap: 6, border: "1.5px solid #e2e8f0" }}>
          <span style={{ color: "#94a3b8" }}>🔍</span>
          <input placeholder="Search patient or drug..." style={{ border: "none", background: "transparent", outline: "none", fontSize: 12, color: "#475569", flex: 1 }} />
        </div>
        {RX_QUEUE.map((r, i) => (
          <div key={i} className={`rx-item ${i === rxSelected ? "active" : ""}`} onClick={() => setRxSelected(i)}>
            <div style={{ width: 36, height: 36, borderRadius: "50%", background: i === rxSelected ? "#2563eb" : "#e2e8f0", display: "flex", alignItems: "center", justifyContent: "center", color: i === rxSelected ? "#fff" : "#475569", fontSize: 12, fontWeight: 700, flexShrink: 0 }}>
              {r.initials}
            </div>
            <div style={{ flex: 1 }}>
              <div style={{ fontWeight: 600, fontSize: 13, color: "#0f172a" }}>{r.name}</div>
              <div style={{ fontSize: 11, color: "#94a3b8" }}>{r.time} · {r.items} item{r.items > 1 ? "s" : ""}</div>
            </div>
            <span className="pill" style={{ background: r.statusColor + "18", color: r.statusColor, fontSize: 10 }}>{r.status}</span>
          </div>
        ))}
      </div>

      {/* Patient detail panel */}
      <div style={{ flex: 1, padding: 28, overflow: "auto" }}>
        {/* Patient header */}
        <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 24 }}>
          <div>
            <h1 style={{ fontSize: 22, fontWeight: 700, color: "#0f172a" }}>{patient.name}</h1>
            <div style={{ fontSize: 13, color: "#64748b", marginTop: 3 }}>
              DOB: 12 Apr 1978 · CNP: 278041XXXXX · Patient since 2023
            </div>
          </div>
          <div style={{ display: "flex", gap: 10 }}>
            <button className="btn" style={{ background: "#fff", border: "1.5px solid #fca5a5", color: "#ef4444", padding: "9px 18px", fontSize: 13 }}>✕ Reject</button>
            <button className="btn" style={{ background: "#0f172a", color: "#fff", padding: "9px 18px", fontSize: 13 }}>✓ Validate Prescription</button>
          </div>
        </div>

        {/* Doctor info */}
        <div className="card" style={{ padding: "16px 18px", marginBottom: 20 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 14 }}>
            <div style={{ width: 42, height: 42, borderRadius: "50%", background: "#eff6ff", display: "flex", alignItems: "center", justifyContent: "center", fontSize: 20 }}>🩺</div>
            <div style={{ flex: 1 }}>
              <div style={{ fontWeight: 700, fontSize: 14, color: "#0f172a" }}>Dr. Florin Neagu</div>
              <div style={{ fontSize: 12, color: "#64748b" }}>Cardiologie · Spitalul Județean Cluj · Issued 25 Mar 2026</div>
            </div>
            <span style={{ color: "#10b981", fontSize: 13, fontWeight: 600 }}>Verified ✓</span>
          </div>
        </div>

        {/* Status row */}
        <div style={{ display: "grid", gridTemplateColumns: "repeat(3,1fr)", gap: 12, marginBottom: 20 }}>
          {[
            { label: "Status",    value: patient.status,      color: patient.statusColor },
            { label: "Items",     value: `${patient.items} medications`, color: "#0f172a" },
            { label: "Submitted", value: patient.time,        color: "#64748b" },
          ].map((s, i) => (
            <div key={i} className="card" style={{ padding: "14px 16px" }}>
              <div style={{ fontSize: 11, color: "#94a3b8", fontWeight: 700, letterSpacing: "0.07em", marginBottom: 6 }}>{s.label.toUpperCase()}</div>
              <div style={{ fontSize: 16, fontWeight: 700, color: s.color }}>{s.value}</div>
            </div>
          ))}
        </div>

        {/* Medication list */}
        <div style={{ fontWeight: 700, fontSize: 12, color: "#94a3b8", letterSpacing: "0.07em", marginBottom: 12 }}>PRESCRIBED MEDICATIONS ({MEDS.length})</div>
        {MEDS.map((m, i) => (
          <div key={i} className="card" style={{ padding: "16px 18px", marginBottom: 10, display: "flex", alignItems: "center", gap: 14 }}>
            <div style={{ width: 44, height: 44, borderRadius: 10, background: "#f8fafc", display: "flex", alignItems: "center", justifyContent: "center", fontSize: 22, flexShrink: 0 }}>{m.emoji}</div>
            <div style={{ flex: 1 }}>
              <div style={{ fontWeight: 700, fontSize: 14, color: "#0f172a" }}>{m.name}</div>
              <div style={{ fontSize: 12, color: "#64748b", marginTop: 2 }}>{m.info}</div>
            </div>
            <div style={{ textAlign: "right" }}>
              <div style={{ fontWeight: 800, fontSize: 15, color: "#0f172a" }}>{m.price}</div>
              <div style={{ fontSize: 12, color: m.color, fontWeight: 600 }}>{m.avail}</div>
            </div>
          </div>
        ))}

        {/* Total */}
        <div style={{ display: "flex", justifyContent: "flex-end", padding: "14px 0", borderTop: "1.5px solid #f1f5f9", marginTop: 6 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 16 }}>
            <span style={{ fontSize: 14, color: "#64748b" }}>Prescription total</span>
            <span style={{ fontSize: 22, fontWeight: 800, color: "#0f172a" }}>72 RON</span>
          </div>
        </div>
      </div>
    </div>
  );
}
