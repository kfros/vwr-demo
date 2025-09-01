import axios from "axios";
export const API_URL =
  (import.meta.env.VITE_API_URL as string | undefined)?.replace(/\/+$/, '') ??
  'http://localhost:5248';
export const api = axios.create({
  baseURL: API_URL
});