import { api } from "./api";
export async function siweLogin(
  message: string,
  signature: string,
  address: string
) {
  const { data } = await api.post("/auth/siwe/verify", {
    message,
    signature,
    address,
  });
  localStorage.setItem("jwt", data.jwt);
}
