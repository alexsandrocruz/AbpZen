import { useState } from "react";
import { Sidebar, SidebarTrigger } from "./sidebar";
import { Bell, Search, Plus, User, ShieldAlert, Monitor, LogOut, ChevronDown, ShieldCheck } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ThemeToggle } from "@/components/ui/theme-toggle";
import { useAuth } from "@/lib/abp/auth";
import { useAbpPermissions } from "@/lib/abp/config";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Link } from "wouter";

interface ShellProps {
    children: React.ReactNode;
    appName?: string;
}

export function AppShell({ children, appName = "AbpReact" }: ShellProps) {
    const [sidebarOpen, setSidebarOpen] = useState(false);
    const { user, logout } = useAuth();
    const { hasPermission } = useAbpPermissions();

    // Check if user has permission for security logs
    const canViewSecurityLogs = hasPermission("AbpIdentity.SecurityLogs") || user?.roles.includes("admin");

    return (
        <div className="min-h-screen bg-background text-foreground">
            <Sidebar
                isOpen={sidebarOpen}
                onClose={() => setSidebarOpen(false)}
                appName={appName}
            />

            {/* Main content area */}
            <div className="lg:pl-64 flex flex-col min-h-screen">
                {/* Top bar */}
                <header className="h-16 border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60 sticky top-0 z-40 px-4 lg:px-6">
                    <div className="flex items-center justify-between h-full w-full">
                        <div className="flex items-center gap-4">
                            <SidebarTrigger onClick={() => setSidebarOpen(true)} />

                            {/* Search */}
                            <div className="hidden md:flex relative w-72">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                <Input
                                    placeholder="Search..."
                                    className="pl-10 h-10 bg-muted/20 border-none focus:bg-background transition-all"
                                />
                            </div>
                        </div>

                        <div className="flex items-center gap-2">
                            <Button size="sm" className="gap-2 hidden sm:flex shadow-lg shadow-primary/20">
                                <Plus className="size-4" />
                                <span>New</span>
                            </Button>

                            <ThemeToggle />

                            <Button variant="ghost" size="icon" className="text-muted-foreground hover:text-primary transition-colors">
                                <Bell className="size-5" />
                            </Button>

                            <DropdownMenu>
                                <DropdownMenuTrigger asChild>
                                    <Button variant="ghost" className="relative h-10 flex items-center gap-2 pl-2 pr-1 rounded-full hover:bg-muted/50 transition-all outline-none">
                                        <Avatar className="h-8 w-8 border border-muted ring-2 ring-background shadow-sm">
                                            <AvatarImage src={`https://api.dicebear.com/7.x/avataaars/svg?seed=${user?.userName}`} />
                                            <AvatarFallback className="bg-primary/10 text-primary font-bold text-xs">
                                                {user?.name?.[0]}{user?.userName?.[0]}
                                            </AvatarFallback>
                                        </Avatar>
                                        <div className="hidden md:flex flex-col items-start mr-1 text-left">
                                            <span className="text-xs font-bold leading-none">{user?.name || user?.userName}</span>
                                            <span className="text-[10px] text-muted-foreground leading-none mt-1">{user?.email}</span>
                                        </div>
                                        <ChevronDown className="size-3 text-muted-foreground mr-1" />
                                    </Button>
                                </DropdownMenuTrigger>
                                <DropdownMenuContent className="w-64 p-2 shadow-2xl border-none mt-1 mr-[-4px] animate-in fade-in zoom-in-95 duration-200" align="end">
                                    <DropdownMenuLabel className="px-3 py-3">
                                        <div className="flex flex-col space-y-1">
                                            <p className="text-sm font-bold leading-none">{user?.name || "Standard User"}</p>
                                            <p className="text-xs leading-none text-muted-foreground">
                                                {user?.email || "admin@abp.io"}
                                            </p>
                                        </div>
                                    </DropdownMenuLabel>
                                    <DropdownMenuSeparator className="bg-muted/50" />
                                    <div className="p-1 space-y-0.5">
                                        <Link href="/profile">
                                            <DropdownMenuItem className="cursor-pointer gap-3 px-3 py-2.5 rounded-lg focus:bg-primary/5 focus:text-primary transition-colors text-sm font-medium">
                                                <User className="size-4 text-muted-foreground" />
                                                <span>Minha conta</span>
                                            </DropdownMenuItem>
                                        </Link>

                                        {canViewSecurityLogs && (
                                            <Link href="/host/security-logs">
                                                <DropdownMenuItem className="cursor-pointer gap-3 px-3 py-2.5 rounded-lg focus:bg-primary/5 focus:text-primary transition-colors text-sm font-medium">
                                                    <ShieldAlert className="size-4 text-muted-foreground" />
                                                    <span>Logs de segurança</span>
                                                </DropdownMenuItem>
                                            </Link>
                                        )}

                                        <Link href="/sessions">
                                            <DropdownMenuItem className="cursor-pointer gap-3 px-3 py-2.5 rounded-lg focus:bg-primary/5 focus:text-primary transition-colors text-sm font-medium">
                                                <Monitor className="size-4 text-muted-foreground" />
                                                <span>Sessions</span>
                                            </DropdownMenuItem>
                                        </Link>

                                        <Link href="/profile/lgpd">
                                            <DropdownMenuItem className="cursor-pointer gap-3 px-3 py-2.5 rounded-lg focus:bg-primary/5 focus:text-primary transition-colors text-sm font-medium">
                                                <ShieldCheck className="size-4 text-muted-foreground" />
                                                <span>Dados Pessoais / LGPD</span>
                                            </DropdownMenuItem>
                                        </Link>
                                    </div>
                                    <DropdownMenuSeparator className="bg-muted/50" />
                                    <DropdownMenuItem
                                        onClick={() => logout()}
                                        className="cursor-pointer gap-3 px-3 py-2.5 rounded-lg text-rose-500 focus:bg-rose-50 focus:text-rose-600 transition-colors m-1 text-sm font-bold"
                                    >
                                        <LogOut className="size-4" />
                                        <span>Sair</span>
                                    </DropdownMenuItem>
                                </DropdownMenuContent>
                            </DropdownMenu>
                        </div>
                    </div>
                </header>

                {/* Page content */}
                <main className="p-4 lg:p-6 flex-1">
                    {children}
                </main>
            </div>
        </div>
    );
}

export { AppShell as Shell };
