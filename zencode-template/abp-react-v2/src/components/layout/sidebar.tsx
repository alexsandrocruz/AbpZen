import { Link, useLocation } from "wouter";
import { cn } from "@/lib/utils";
import {
    LayoutDashboard,
    Users,
    Settings,
    LogOut,
    Building2,
    Shield,
    X,
    Menu,
} from "lucide-react";
import { Button } from "@/components/ui/button";

interface SidebarProps {
    isOpen?: boolean;
    onClose?: () => void;
    appName?: string;
}

export function Sidebar({ isOpen = true, onClose, appName = "AbpReact" }: SidebarProps) {
    const [location] = useLocation();

    // Main navigation items
    const navItems = [
        { label: "Dashboard", icon: LayoutDashboard, href: "/dashboard" },
    ];

    // Host administration items (ABP)
    const hostItems = [
        { label: "Workspaces", icon: Building2, href: "/host/workspaces" },
        { label: "Users", icon: Users, href: "/host/users" },
        { label: "Roles", icon: Shield, href: "/host/roles" },
    ];

    return (
        <>
            {/* Backdrop for mobile */}
            {isOpen && onClose && (
                <div
                    className="fixed inset-0 bg-black/50 z-40 lg:hidden"
                    onClick={onClose}
                />
            )}

            <div
                className={cn(
                    "h-screen w-64 border-r bg-sidebar text-sidebar-foreground flex flex-col fixed left-0 top-0 z-50 transition-transform duration-300 ease-in-out",
                    "lg:translate-x-0 lg:z-30",
                    isOpen ? "translate-x-0" : "-translate-x-full"
                )}
            >
                {/* Header */}
                <div className="h-16 flex items-center justify-between px-6 border-b border-sidebar-border">
                    <div className="flex items-center gap-2 font-display font-bold text-xl tracking-tight">
                        <div className="size-8 rounded-lg bg-primary text-primary-foreground flex items-center justify-center">
                            <LayoutDashboard className="size-5" />
                        </div>
                        <span>{appName}</span>
                    </div>
                    {onClose && (
                        <button
                            onClick={onClose}
                            className="lg:hidden p-1 rounded-md hover:bg-sidebar-accent"
                        >
                            <X className="size-5" />
                        </button>
                    )}
                </div>

                {/* Navigation */}
                <div className="flex-1 py-6 px-3 space-y-1 overflow-y-auto">
                    {navItems.map((item) => {
                        const isActive = location === item.href || location.startsWith(`${item.href}/`);
                        return (
                            <Link key={item.href} href={item.href}>
                                <div
                                    className={cn(
                                        "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group relative cursor-pointer",
                                        isActive
                                            ? "bg-sidebar-accent text-sidebar-accent-foreground shadow-sm"
                                            : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                                    )}
                                >
                                    <item.icon className={cn("size-5", isActive ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                                    {item.label}
                                    {isActive && (
                                        <div className="absolute right-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-l-full" />
                                    )}
                                </div>
                            </Link>
                        );
                    })}

                    {/* Host Administration Section */}
                    <div className="pt-4 mt-4 border-t border-sidebar-border">
                        <span className="px-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                            Host Admin
                        </span>
                        <div className="mt-2 space-y-1">
                            {hostItems.map((item) => {
                                const isActive = location === item.href || location.startsWith(`${item.href}/`);
                                return (
                                    <Link key={item.href} href={item.href}>
                                        <div
                                            className={cn(
                                                "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group relative cursor-pointer",
                                                isActive
                                                    ? "bg-sidebar-accent text-sidebar-accent-foreground shadow-sm"
                                                    : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                                            )}
                                        >
                                            <item.icon className={cn("size-5", isActive ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                                            {item.label}
                                            {isActive && (
                                                <div className="absolute right-0 top-1/2 -translate-y-1/2 w-1 h-6 bg-primary rounded-l-full" />
                                            )}
                                        </div>
                                    </Link>
                                );
                            })}
                        </div>
                    </div>
                </div>

                {/* Footer */}
                <div className="p-3 border-t border-sidebar-border space-y-1">
                    <Link href="/settings">
                        <div
                            className={cn(
                                "flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors cursor-pointer",
                                location.includes("/settings")
                                    ? "bg-sidebar-accent text-sidebar-accent-foreground"
                                    : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                            )}
                        >
                            <Settings className="size-5" />
                            Settings
                        </div>
                    </Link>
                    <button
                        onClick={() => console.log('Logout')}
                        className="w-full flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium text-muted-foreground hover:bg-destructive/10 hover:text-destructive transition-colors"
                    >
                        <LogOut className="size-5" />
                        Logout
                    </button>
                </div>
            </div>
        </>
    );
}

// Mobile menu trigger
export function SidebarTrigger({ onClick }: { onClick: () => void }) {
    return (
        <Button variant="ghost" size="icon" onClick={onClick} className="lg:hidden">
            <Menu className="size-5" />
        </Button>
    );
}
