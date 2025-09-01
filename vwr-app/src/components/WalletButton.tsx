import { useAccount, useConnect, useDisconnect } from "wagmi";
export function WalletButton(){
  const { isConnected, address } = useAccount();
  const { connectors, connect, isPending } = useConnect();
  const { disconnect } = useDisconnect();
  if(!isConnected) return <button onClick={()=>connect({ connector: connectors[0] })} disabled={isPending}>Connect</button>;
  return <button onClick={()=>disconnect()}>{address?.slice(0,6)}… Disconnect</button>;
}