import React from "react";
import { Link, useLocation } from 'react-router-dom';
import { LayoutDashboard, Box, History, ShieldAlert } from "lucide-react";

//mainlayout sayfam cnm tüm sayfaları içinde barındıran sayfa cnm

export default function MainLayout ({ children }) {
  const location = useLocation();

  const navItems = [
    { name : 'Dashboard' , path :'/inventory', icon: <LayoutDashboard size= {20}/> },
    { name : 'Şantiyeler' , path: '/sites', icon: <Box size ={20}/>},
    {name : 'Stok Hareketleri', path: '/movements' , icon: <History size={20}/>}
    

  ]
}
