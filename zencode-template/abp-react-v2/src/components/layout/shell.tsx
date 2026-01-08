import { useState } from "react";
import { Sidebar, SidebarTrigger } from "./sidebar";
import { Bell, Search, Plus, User } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ThemeToggle } from "@/components/ui/theme-toggle";

interface ShellProps {
    children: React.ReactNode;
    appName?: string;
}

export function AppShell({ children, appName = "AbpReact" }: ShellProps) {
    const [sidebarOpen, setSidebarOpen] = useState(false);

    return (
        <div className="min-h-screen bg-background">
            <Sidebar
                isOpen={sidebarOpen}
                onClose={() => setSidebarOpen(false)}
                appName={appName}
            />

            {/* Main content area */}
            <div className="lg:pl-64">
                {/* Top bar */}
                <header className="h-16 border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60 sticky top-0 z-40">
                    <div className="flex items-center justify-between h-full px-4 lg:px-6">
                        <div className="flex items-center gap-4">
                            <SidebarTrigger onClick={() => setSidebarOpen(true)} />

                            {/* Search */}
                            <div className="hidden md:flex relative w-72">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                <Input
                                    placeholder="Search..."
                                    className="pl-10"
                                />
                            </div>
                        </div>

                        <div className="flex items-center gap-2">
                            <Button size="sm" className="gap-2">
                                <Plus className="size-4" />
                                <span className="hidden sm:inline">New</span>
                            </Button>

                            <ThemeToggle />

                            <Button variant="ghost" size="icon">
                                <Bell className="size-5" />
                            </Button>

                            <Button variant="ghost" size="icon" className="rounded-full" asChild>
                                <a href="/profile">
                                    <User className="size-5" />
                                </a>
                            </Button>
                        </div>
                    </div>
                </header>

                {/* Page content */}
                <main className="p-4 lg:p-6">
                    {children}
                </main>
            </div>
        </div>
    );
}
