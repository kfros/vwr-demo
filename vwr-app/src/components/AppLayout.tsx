import {Outlet, Link} from "@tanstack/react-router";
import { WalletButton } from "./WalletButton";

export default function AppLayout() {
    return (
        <div className="min-h-screen flex flex-col">
            {/* Header */}
            <header className="sticky top-0 z-10 border-b bg-white/80 backdrop-blur">
        <div className="container-page h-16 flex items-center justify-between">
          <Link to="/" className="text-xl font-semibold tracking-tight">VWR</Link>
          <WalletButton />
        </div>
      </header>

      <main className="flex-1 py-8">
        <div className="container-page">
          <Outlet />
        </div>
      </main>

      <footer className="py-8 text-center text-sm text-slate-500">
        built with .NET • React • Hardhat
      </footer>
      </div>
    )}