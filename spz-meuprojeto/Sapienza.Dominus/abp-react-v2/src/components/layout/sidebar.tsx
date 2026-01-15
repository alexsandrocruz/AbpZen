import { Link, useLocation } from "wouter";
import { cn } from "@/lib/utils";
import { useState, useEffect } from "react";
import {
    LayoutDashboard,
    LogOut,
    Settings,
    X,
    Menu,
    ChevronRight,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { menuItems, type NavItem } from "@/config/navigation";
import * as Collapsible from "@radix-ui/react-collapsible";
import { useAbpPermissions } from "@/lib/abp/config";

interface SidebarProps {
    isOpen?: boolean;
    onClose?: () => void;
    appName?: string;
}

export function Sidebar({ isOpen = true, onClose, appName = "AbpReact" }: SidebarProps) {
    const [location] = useLocation();
    const { currentTenant } = useAbpPermissions();
    const isHost = !currentTenant?.id || currentTenant?.id === null;

    // Group items by section
    const mainItems = menuItems.filter(item => item.section === "main").map(item => {
        // If tenant, point Dashboard to tenant-dashboard
        if (item.label === "Dashboard" && !isHost) {
            return { ...item, href: "/tenant-dashboard" };
        }
        return item;
    });
    const adminItems = menuItems.filter(item => item.section === "admin");
    const hostItems = isHost ? menuItems.filter(item => item.section === "host") : [];
    const entityItems = menuItems.filter(item => item.section === "entities");

    const renderNavItem = (item: NavItem) => {
        if (item.items && item.items.length > 0) {
            return <CollapsibleNavItem key={item.label} item={item} currentLocation={location} />;
        }

        const isActive = location === item.href || (item.href !== "/" && location.startsWith(`${item.href}/`));

        return (
            <Link key={item.href} href={item.href!}>
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
    };

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
                <div className="flex-1 py-6 px-3 space-y-1 overflow-y-auto custom-scrollbar">
                    {mainItems.map(renderNavItem)}

                    {hostItems.length > 0 && (
                        <div className="pt-4 mt-4 border-t border-sidebar-border">
                            <span className="px-3 text-[10px] font-bold text-muted-foreground uppercase tracking-widest opacity-50">
                                Host Admin
                            </span>
                            <div className="mt-2 space-y-1">
                                {hostItems.map(renderNavItem)}
                            </div>
                        </div>
                    )}

                    {entityItems.length > 0 && (
                        <div className="pt-4 mt-4 border-t border-sidebar-border">
                            <span className="px-3 text-[10px] font-bold text-muted-foreground uppercase tracking-widest opacity-50">
                                Entities
                            </span>
                            <div className="mt-2 space-y-1">
                                {entityItems.map(renderNavItem)}
                            </div>
                        </div>
                    )}

                    {adminItems.length > 0 && (
                        <div className="pt-4 mt-4 border-t border-sidebar-border">
                            <span className="px-3 text-[10px] font-bold text-muted-foreground uppercase tracking-widest opacity-50">
                                Administration
                            </span>
                            <div className="mt-2 space-y-1">
                                {adminItems.map(renderNavItem)}
                            </div>
                        </div>
                    )}
                </div>

                {/* Footer */}
                <div className="p-3 border-t border-sidebar-border space-y-1">
                    <Link href="/host/settings">
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

function CollapsibleNavItem({ item, currentLocation }: { item: NavItem, currentLocation: string }) {
    const isChildActive = item.items?.some(child =>
        currentLocation === child.href || (child.href !== "/" && currentLocation.startsWith(`${child.href}/`))
    );

    const [isOpen, setIsOpen] = useState(isChildActive);

    // Auto-expand if a child is active
    useEffect(() => {
        if (isChildActive) setIsOpen(true);
    }, [isChildActive]);

    return (
        <Collapsible.Root open={isOpen} onOpenChange={setIsOpen} className="w-full">
            <Collapsible.Trigger asChild>
                <div
                    className={cn(
                        "flex items-center justify-between gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-all duration-200 group cursor-pointer",
                        isOpen
                            ? "text-sidebar-foreground bg-sidebar-accent/30"
                            : "text-muted-foreground hover:bg-sidebar-accent/50 hover:text-sidebar-foreground"
                    )}
                >
                    <div className="flex items-center gap-3">
                        <item.icon className={cn("size-5", isOpen ? "text-primary" : "text-muted-foreground group-hover:text-primary")} />
                        {item.label}
                    </div>
                    <ChevronRight className={cn("size-4 transition-transform duration-200", isOpen && "rotate-90")} />
                </div>
            </Collapsible.Trigger>

            <Collapsible.Content className="pl-9 pr-2 space-y-1 mt-1 overflow-hidden data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[side=bottom]:slide-in-from-top-2">
                {item.items?.map(child => {
                    const isActive = currentLocation === child.href || (child.href !== "/" && currentLocation.startsWith(`${child.href}/`));
                    return (
                        <Link key={child.href} href={child.href!}>
                            <div
                                className={cn(
                                    "px-3 py-2 rounded-md text-xs font-medium transition-all duration-200 cursor-pointer",
                                    isActive
                                        ? "bg-primary/10 text-primary"
                                        : "text-muted-foreground hover:text-foreground hover:bg-sidebar-accent/30"
                                )}
                            >
                                {child.label}
                            </div>
                        </Link>
                    );
                })}
            </Collapsible.Content>
        </Collapsible.Root>
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
