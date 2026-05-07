import { RECENT_ORDERS, STOCK_ALERTS, HOURLY } from "./data.js";

export default function DashboardPage({ setPage }) {
  const maxH = Math.max(...HOURLY);

  return (
    <div style={{ padding: 28 }}>
      {/* Header */}
      <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 24 }}>
        <div>
          <h1 style={{ fontSize: 22, fontWeight: 700, color: "#0f172a" }}>Good morning, Alex 👋</h1>
          <p style={{ color: "#64748b", fontSize: 14, marginTop: 2 }}>Here's what's happening across your network today.</p>
        </div>
        <div style={{ display: "flex", gap: 10 }}>
          <div style={{ background: "#fff", border: "1.5px solid #e2e8f0", borderRadius: 8, padding: "8px 14px", fontSize: 13, color: "#475569", fontWeight: 500 }}>Wed, 25 Mar 2026</div>
          <div style={{ background: "#fff", border: "1.5px solid #e2e8f0", borderRadius: 8, padding: "8px 12px", fontSize: 16 }}>🔔</div>
          <button className="btn" style={{ background: "#0f172a", color: "#fff", padding: "8px 16px", fontSize: 13 }}>Export Report</button>
        </div>
      </div>

      {/* KPI cards */}
      <div style={{ display: "grid", gridTemplateColumns: "repeat(4,1fr)", gap: 14, marginBottom: 20 }}>
        {[
          { icon: "📦", value: "148",   label: "Orders today",    badge: "↑ 12%",  bColor: "#dcfce7", bText: "#166534" },
          { icon: "💰", value: "9,240", label: "Revenue (RON)",   badge: "↑ 8%",   bColor: "#dcfce7", bText: "#166534" },
          { icon: "⚠️", value: "7",     label: "Low stock items", badge: "Urgent", bColor: "#fef3c7", bText: "#92400e" },
          { icon: "👥", value: "1,382", label: "Active patients", badge: "+23",    bColor: "#dbeafe", bText: "#1e40af" },
        ].map((k, i) => (
          <div key={i} className="card" style={{ padding: "20px 18px" }}>
            <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 10 }}>
              <span style={{ fontSize: 22 }}>{k.icon}</span>
              <span className="pill" style={{ background: k.bColor, color: k.bText, fontSize: 11 }}>{k.badge}</span>
            </div>
            <div style={{ fontSize: 28, fontWeight: 800, color: "#0f172a" }}>{k.value}</div>
            <div style={{ fontSize: 13, color: "#64748b", marginTop: 2 }}>{k.label}</div>
          </div>
        ))}
      </div>

      {/* Recent Orders + Stock Alerts */}
      <div style={{ display: "grid", gridTemplateColumns: "1fr 340px", gap: 16, marginBottom: 16 }}>
        {/* Recent Orders table */}
        <div className="card" style={{ padding: 20 }}>
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
            <h2 style={{ fontWeight: 700, fontSize: 15, color: "#0f172a" }}>Recent Orders</h2>
            <span style={{ color: "#2563eb", fontSize: 13, cursor: "pointer", fontWeight: 600 }}>View all →</span>
          </div>
          <table style={{ width: "100%", borderCollapse: "collapse" }}>
            <thead>
              <tr style={{ borderBottom: "1.5px solid #f1f5f9" }}>
                {["ORDER", "PATIENT", "ITEMS", "TOTAL", "STATUS"].map(h => (
                  <th key={h} style={{ textAlign: "left", padding: "6px 8px", fontSize: 11, color: "#94a3b8", fontWeight: 700, letterSpacing: "0.05em" }}>{h}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {RECENT_ORDERS.map((o, i) => (
                <tr key={i} style={{ borderBottom: "1px solid #f8fafc" }}>
                  <td style={{ padding: "12px 8px", fontSize: 13, color: "#475569", fontWeight: 600 }}>{o.id}</td>
                  <td style={{ padding: "12px 8px", fontSize: 13, color: "#0f172a", fontWeight: 500 }}>{o.patient}</td>
                  <td style={{ padding: "12px 8px", fontSize: 13, color: "#475569" }}>{o.items}</td>
                  <td style={{ padding: "12px 8px", fontSize: 13, color: "#0f172a", fontWeight: 600 }}>{o.total}</td>
                  <td style={{ padding: "12px 8px" }}>
                    <span className="pill" style={{ background: o.color + "20", color: o.color, fontSize: 11 }}>{o.status}</span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Stock Alerts */}
        <div className="card" style={{ padding: 20 }}>
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 14 }}>
            <h2 style={{ fontWeight: 700, fontSize: 15, color: "#0f172a" }}>Stock Alerts</h2>
            <span style={{ color: "#2563eb", fontSize: 13, cursor: "pointer", fontWeight: 600 }}>Reorder all</span>
          </div>
          {STOCK_ALERTS.map((s, i) => (
            <div key={i} style={{ background: "#fefce8", borderRadius: 10, padding: "12px 14px", marginBottom: 10 }}>
              <div style={{ fontWeight: 600, fontSize: 13, color: "#0f172a", marginBottom: 6 }}>{s.name}</div>
              <div style={{ background: "#e2e8f0", borderRadius: 99, height: 6, marginBottom: 4, overflow: "hidden" }}>
                <div style={{ height: "100%", borderRadius: 99, width: `${s.pct * 100}%`, background: s.pct < 0.3 ? "#ef4444" : "#f59e0b" }} />
              </div>
              <div style={{ fontSize: 11, color: "#64748b" }}>{s.current} units · min. {s.min}</div>
            </div>
          ))}
        </div>
      </div>

      {/* Hourly Sales chart */}
      <div className="card" style={{ padding: 20 }}>
        <h2 style={{ fontWeight: 700, fontSize: 15, color: "#0f172a", marginBottom: 16 }}>Hourly Sales Today</h2>
        <div style={{ display: "flex", alignItems: "flex-end", gap: 8, height: 100 }}>
          {HOURLY.map((v, i) => (
            <div key={i} style={{ flex: 1, display: "flex", flexDirection: "column", alignItems: "center", gap: 6 }}>
              <div style={{ width: "100%", height: `${(v / maxH) * 80}px`, background: v > 40 ? "#2563eb" : "#bfdbfe", borderRadius: "4px 4px 0 0", transition: "all .3s" }} />
              <span style={{ fontSize: 10, color: "#94a3b8" }}>{8 + i}am</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
