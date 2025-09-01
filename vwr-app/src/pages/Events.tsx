import React from "react";
import { api } from "../lib/api";

type Source = "github" | "slack" | "jira" | "demo";

export default function Events() {
  const [events, setEvents] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);
  const [posting, setPosting] = React.useState(false);

  // --- select source
  const [source, setSource] = React.useState<Source>("github");

  // --- issue counter with saving to localStorage
  const [issue, setIssue] = React.useState<number>(() => {
    const v = localStorage.getItem("vwr.issue");
    return v ? Number(v) || 1 : 1;
  });
  React.useEffect(() => {
    localStorage.setItem("vwr.issue", String(issue));
  }, [issue]);

 const baseUrl = React.useMemo(
    () => api.defaults.baseURL!.replace(/\/+$/, ""),
    []
  );
  
const fetchEvents = React.useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const res = await fetch(`${baseUrl}/api/Events`, { method: "GET", mode: "cors" });
      if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
      const data = await res.json();
      setEvents(data);

      // Try to parse payloads as JSON and extract max `issue` number, if any.
      let nextIssue = issue;
      for (const e of data) {
        try {
          const parsed = JSON.parse(e.payload);
          if (parsed && typeof parsed.issue === "number") {
            if (parsed.issue >= nextIssue) nextIssue = parsed.issue + 1;
          }
        } catch {
          // payload isn't JSON — ignore gracefully
        }
      }
      if (nextIssue !== issue) {
        setIssue(nextIssue);
        localStorage.setItem("vwr.issue", String(nextIssue));
      }
    } catch (e: any) {
      setError(e.message ?? String(e));
    } finally {
      setLoading(false);
    }
  }, [baseUrl, issue]);

  React.useEffect(() => {
    fetchEvents();
  }, [fetchEvents]);

  const sendWebhook = async () => {
    setPosting(true);
    setError(null);
    try {
      const base = api.defaults.baseURL!.replace(/\/+$/, "");
      const url = `${base}/api/webhooks/in/${encodeURIComponent(source)}`;

      
      const payload =
        source === "github"
          ? { action: "opened", issue, repo: "vwr/frontend", from: "ui" }
          : source === "slack"
          ? { action: "message", issue, channel: "#general", text: "hello from ui" }
          : source === "jira"
          ? { action: "created", issue, project: "VWR", summary: "Demo ticket from UI" }
          : { action: "demo", issue, note: "generic demo webhook" };

      const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      const text = await res.text();
      if (!res.ok) throw new Error(`Webhook error: ${res.status} ${text}`);

      
      setIssue((i) => i + 1);
      fetchEvents();
    } catch (e: any) {
      setError(e.message ?? String(e));
    } finally {
      setPosting(false);
    }
  };

  if (loading) return <div className="card">Загрузка…</div>;
  if (error) return <div className="card text-red-600">Ошибка: {error}</div>;

  return (
    <section className="space-y-6">
      <div className="card flex flex-col md:flex-row md:items-center gap-3 justify-between">
        <div className="flex items-center gap-3">
          <h2 className="text-xl font-semibold">Recent Events</h2>
          <div className="flex items-center gap-2">
            <label className="text-sm text-slate-600">Source:</label>
            <select
              className="border rounded-lg px-3 py-1.5 bg-white"
              value={source}
              onChange={(e) => setSource(e.target.value as Source)}
            >
              <option value="github">github</option>
              <option value="slack">slack</option>
              <option value="jira">jira</option>
              <option value="demo">demo</option>
            </select>
          </div>
          <div className="text-sm text-slate-600 hidden md:block">•</div>
          <div className="text-sm text-slate-600">next issue: <b>#{issue}</b></div>
        </div>

        <div className="flex items-center gap-2">
          <button
            className="btn btn-primary"
            onClick={sendWebhook}
            disabled={posting}
            title={`POST /api/webhooks/in/${source}`}
          >
            {posting ? "Sending..." : `Send webhook (${source}, issue #${issue})`}
          </button>
          <button
            className="btn btn-ghost"
            onClick={() => setIssue(1)}
            disabled={posting}
            title="Reset counter"
          >
            Reset issue
          </button>
        </div>
      </div>

      <div className="space-y-3">
        {events.map((e) => (
          <div key={e.id} className="card">
            <div className="text-sm text-slate-500 mb-2">
              <b>{e.source}</b> — {new Date(e.receivedAt).toLocaleString()}
            </div>
            <pre className="text-xs bg-slate-50 border rounded-xl p-3 overflow-x-auto">
              {e.payload}
            </pre>
          </div>
        ))}
        {events.length === 0 && (
          <div className="card text-slate-500">Пока пусто — отправьте вебхук.</div>
        )}
      </div>
    </section>
  );
}
