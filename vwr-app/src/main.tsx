import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import {
  createRouter,
  createRoute,
  createRootRoute,
  RouterProvider,
} from "@tanstack/react-router";

import { WagmiProvider, createConfig, http } from "wagmi";
import { injected } from "wagmi/connectors";
import { foundry } from "viem/chains";
import { defineChain } from "viem";

import AppLayout from "./components/AppLayout";
import Home from "./pages/Home";
import Events from "./pages/Events";
import { Receipts } from "./pages/Receipts";

// wagmi / viem
const LOCAL = defineChain({
  ...foundry,
  id: Number(import.meta.env.VITE_CHAIN_ID ?? 31337),
  name: "Local",
});
const wagmiConfig = createConfig({
  chains: [LOCAL],
  connectors: [injected()],
  transports: { [LOCAL.id]: http(import.meta.env.VITE_RPC_URL ?? "http://127.0.0.1:8545") },
  multiInjectedProviderDiscovery: true,
  ssr: false,
});

// tanstack router
const rootRoute = createRootRoute({ component: AppLayout });
const indexRoute = createRoute({ getParentRoute: () => rootRoute, path: "/", component: Home });
const eventsRoute = createRoute({ getParentRoute: () => rootRoute, path: "/events", component: Events });
const receiptsRoute = createRoute({ getParentRoute: () => rootRoute, path: "/receipts", component: Receipts });
const routeTree = rootRoute.addChildren([indexRoute, eventsRoute, receiptsRoute]);
const router = createRouter({ routeTree });

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <WagmiProvider config={wagmiConfig}>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </WagmiProvider>
  </React.StrictMode>
);
