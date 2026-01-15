import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Users, Building2, Shield, Activity, Download, Calendar, Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { OverviewChart, RequestsChart } from "@/components/dashboard/charts";
import { ActivityFeed } from "@/components/dashboard/activity-feed";
import { useDashboardStats } from "@/lib/abp/hooks/use-dashboard-stats";
import { Skeleton } from "@/components/ui/skeleton";

const statConfig = [
    {
        key: "totalUsers",
        title: "Total Users",
        description: "Registered in platform",
        icon: Users,
        color: "text-blue-500",
        bgColor: "bg-blue-500/10",
    },
    {
        key: "totalWorkspaces",
        title: "Workspaces",
        description: "Active tenants",
        icon: Building2,
        color: "text-emerald-500",
        bgColor: "bg-emerald-500/10",
    },
    {
        key: "totalRoles",
        title: "Active Roles",
        description: "System-wide",
        icon: Shield,
        color: "text-purple-500",
        bgColor: "bg-purple-500/10",
    },
    {
        key: "apiRequests",
        title: "API Requests",
        description: "This month",
        icon: Activity,
        color: "text-amber-500",
        bgColor: "bg-amber-500/10",
    },
];

export default function DashboardPage() {
    const { stats, isLoading } = useDashboardStats();

    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
                        <p className="text-muted-foreground">
                            Welcome back! Here's an overview of your system analytics.
                        </p>
                    </div>
                    <div className="flex items-center gap-2">
                        <Button variant="outline" size="sm" className="gap-2">
                            <Calendar className="size-4" />
                            Jan 2024 - Dec 2024
                        </Button>
                        <Button size="sm" className="gap-2">
                            <Download className="size-4" />
                            Export Report
                        </Button>
                    </div>
                </div>

                {/* Stats Grid */}
                <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    {statConfig.map((stat) => (
                        <Card key={stat.key} className="hover:shadow-lg transition-shadow duration-300">
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                                <CardTitle className="text-sm font-medium">
                                    {stat.title}
                                </CardTitle>
                                <div className={`p-2 rounded-lg ${stat.bgColor}`}>
                                    <stat.icon className={`size-4 ${stat.color}`} />
                                </div>
                            </CardHeader>
                            <CardContent>
                                {isLoading ? (
                                    <div className="space-y-2">
                                        <Skeleton className="h-8 w-20" />
                                        <Skeleton className="h-3 w-28" />
                                    </div>
                                ) : (
                                    <>
                                        <div className="text-2xl font-bold">
                                            {stats?.[stat.key as keyof typeof stats]?.toLocaleString() ?? "-"}
                                        </div>
                                        <p className="text-xs text-muted-foreground">
                                            {stat.description}
                                        </p>
                                    </>
                                )}
                            </CardContent>
                        </Card>
                    ))}
                </div>

                {/* Main Dashboard Content */}
                <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-7">
                    {/* Charts Section */}
                    <Card className="lg:col-span-4">
                        <CardHeader>
                            <CardTitle>Overview</CardTitle>
                            <CardDescription>
                                User acquisition and growth over time.
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="pl-2">
                            <OverviewChart />
                        </CardContent>
                    </Card>

                    {/* Activity Section */}
                    <Card className="lg:col-span-3">
                        <CardHeader>
                            <CardTitle>Recent Activity</CardTitle>
                            <CardDescription>
                                System events and administrative actions.
                            </CardDescription>
                        </CardHeader>
                        <CardContent>
                            <ActivityFeed />
                        </CardContent>
                    </Card>

                    {/* Secondary Chart Section */}
                    <Card className="lg:col-span-3">
                        <CardHeader>
                            <CardTitle>API Performance</CardTitle>
                            <CardDescription>
                                Request volume by day of week.
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="pl-2">
                            <RequestsChart />
                        </CardContent>
                    </Card>

                    {/* System Status / Quick Actions */}
                    <Card className="lg:col-span-4">
                        <CardHeader>
                            <CardTitle>System Health</CardTitle>
                            <CardDescription>
                                Infrastructure and database status.
                            </CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-4">
                                <div className="flex items-center justify-between p-3 rounded-lg border bg-emerald-500/5 border-emerald-500/10">
                                    <div className="flex items-center gap-3">
                                        <div className="size-2 rounded-full bg-emerald-500 animate-pulse" />
                                        <span className="text-sm font-medium">API Server</span>
                                    </div>
                                    <span className="text-xs font-medium text-emerald-600">Operational</span>
                                </div>
                                <div className="flex items-center justify-between p-3 rounded-lg border bg-emerald-500/5 border-emerald-500/10">
                                    <div className="flex items-center gap-3">
                                        <div className="size-2 rounded-full bg-emerald-500" />
                                        <span className="text-sm font-medium">Database (PostgreSQL)</span>
                                    </div>
                                    <span className="text-xs font-medium text-emerald-600">Healthy</span>
                                </div>
                                <div className="flex items-center justify-between p-3 rounded-lg border bg-amber-500/5 border-amber-500/10">
                                    <div className="flex items-center gap-3">
                                        <div className="size-2 rounded-full bg-amber-500" />
                                        <span className="text-sm font-medium">Redis Cache</span>
                                    </div>
                                    <span className="text-xs font-medium text-amber-600">Maintenance</span>
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </AppShell>
    );
}
