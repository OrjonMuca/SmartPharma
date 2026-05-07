export const NAV_ITEMS = [
  { id: "dashboard", label: "Overview", icon: "▦" },
  { id: "inventory", label: "Inventory", icon: "📦" },
  { id: "orders", label: "Orders", icon: "🗒" },
  { id: "prescriptions", label: "Prescriptions", icon: "💊" },
  { id: "users", label: "Users", icon: "" },
];

export const REPORTS_ITEMS = [
  { id: "analytics", label: "Analytics", icon: "📊" },
  { id: "finance", label: "Finance", icon: "💰" },
];

export const INVENTORY_DATA = [
  { name: "Amoxicillin 500mg", category: "Antibiotic", sku: "SP-0041", stock: 12, reorderAt: 50, unitPrice: "8.40 RON", status: "Critical", level: 12 / 200 },
  { name: "Metformin 850mg", category: "Diabetes", sku: "SP-0118", stock: 28, reorderAt: 100, unitPrice: "6.20 RON", status: "Low", level: 28 / 200 },
  { name: "Atorvastatin 20mg", category: "Cardiology", sku: "SP-0204", stock: 40, reorderAt: 80, unitPrice: "11.30 RON", status: "Low", level: 40 / 200 },
  { name: "Augmentin 875/125mg", category: "Antibiotic", sku: "SP-0039", stock: 156, reorderAt: 50, unitPrice: "22.50 RON", status: "In Stock", level: 156 / 200 },
  { name: "Concor 5mg", category: "Cardiology", sku: "SP-0177", stock: 203, reorderAt: 60, unitPrice: "9.80 RON", status: "In Stock", level: 1 },
  { name: "Vitamin D3 4000 IU", category: "Supplement", sku: "SP-0512", stock: 0, reorderAt: 30, unitPrice: "14.20 RON", status: "Out of Stock", level: 0 },
  { name: "Omega-3 1000mg", category: "Supplement", sku: "SP-0498", stock: 88, reorderAt: 40, unitPrice: "18.60 RON", status: "In Stock", level: 88 / 200 },
];

export const CATALOG_DATA = [
  { name: "Augmentin 875/125mg", sub: "Antibiotic · 14 tabs", price: 48, badge: "Rx required", badgeColor: "#f59e0b", inStock: true, emoji: "💊" },
  { name: "Magnesium B6 Forte", sub: "Supplement · 50 tabs", price: 32, badge: "OTC", badgeColor: "#10b981", inStock: true, emoji: "🌿" },
  { name: "Concor 5mg Tablets", sub: "Cardiology · 30 tabs", price: 26, badge: "Rx required", badgeColor: "#f59e0b", inStock: true, emoji: "❤️" },
  { name: "Vitamin D3 4000 IU", sub: "Supplement · 60 caps", price: 29, badge: "OTC", badgeColor: "#10b981", inStock: true, emoji: "☀️" },
  { name: "Sertraline 50mg", sub: "Mental Health · 28 tabs", price: 55, badge: "Rx required", badgeColor: "#f59e0b", inStock: false, emoji: "🧠" },
  { name: "Metformin 1000mg", sub: "Diabetes · 30 tabs", price: 18, badge: "Rx required", badgeColor: "#f59e0b", inStock: true, emoji: "🩺" },
  { name: "Omega-3 1000mg", sub: "Supplement · 90 caps", price: 44, badge: "OTC", badgeColor: "#10b981", inStock: true, emoji: "🧴" },
  { name: "Aspenter 75mg", sub: "Cardiology · 28 tabs", price: 12, badge: "OTC", badgeColor: "#10b981", inStock: true, emoji: "💉" },
];

export const RX_QUEUE = [
  { initials: "MP", name: "Maria Popescu", time: "10 min ago", items: 3, status: "Pending", statusColor: "#f59e0b" },
  { initials: "AI", name: "Alexandru Ionescu", time: "25 min ago", items: 1, status: "Urgent", statusColor: "#ef4444" },
  { initials: "ED", name: "Elena Dragomir", time: "1h ago", items: 2, status: "Waiting", statusColor: "#6b7280" },
  { initials: "GM", name: "Gheorghe Milea", time: "2h ago", items: 4, status: "Waiting", statusColor: "#6b7280" },
  { initials: "IC", name: "Ioana Chirilă", time: "3h ago", items: 2, status: "Ready", statusColor: "#10b981" },
];

export const RECENT_ORDERS = [
  { id: "#4821", patient: "Agim", items: 3, total: "89 RON", status: "Completed", color: "#10b981" },
  { id: "#4820", patient: "Fatjon", items: 1, total: "34 RON", status: "Processing", color: "#3b82f6" },
  { id: "#4819", patient: "Besmir", items: 5, total: "210 RON", status: "Pending Rx", color: "#f59e0b" },
  { id: "#4818", patient: "Mateo", items: 2, total: "67 RON", status: "Completed", color: "#10b981" },
  { id: "#4817", patient: "Roel", items: 4, total: "145 RON", status: "Cancelled", color: "#ef4444" },
];

export const STOCK_ALERTS = [
  { name: "Amoxicillin 500mg", current: 12, min: 50, pct: 12 / 50 },
  { name: "Metformin 850mg", current: 28, min: 100, pct: 28 / 100 },
  { name: "Atorvastatin 20mg", current: 40, min: 80, pct: 40 / 80 },
];

export const HOURLY = [8, 12, 14, 17, 22, 52, 68, 75, 58, 40, 30, 22];

export function statusColor(s) {
  if (s === "Critical") return "#ef4444";
  if (s === "Low") return "#f59e0b";
  if (s === "Out of Stock") return "#ef4444";
  return "#10b981";
}

export function levelColor(s) {
  if (s === "Critical") return "#ef4444";
  if (s === "Low") return "#f59e0b";
  if (s === "Out of Stock") return "#6b7280";
  return "#10b981";
}
