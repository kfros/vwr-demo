import { createRouter, createRoute, createRootRoute } from '@tanstack/react-router'

// Страницы можете импортировать откуда угодно
const rootRoute = createRootRoute()

export const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: () => <div className="p-6">VWR Home</div>,
})

export const eventsRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/events',
  component: () => <div className="p-6">Events page</div>,
})

const routeTree = rootRoute.addChildren([indexRoute, eventsRoute])
export const router = createRouter({ routeTree })
