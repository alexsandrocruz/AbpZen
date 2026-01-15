import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Users, Activity, Loader2, CheckCircle2, AlertCircle, Clock } from "lucide-react";
import { RequestsChart } from "@/components/dashboard/charts";
import { ActivityFeed } from "@/components/dashboard/activity-feed";
import { useDashboardStats } from "@/lib/abp/hooks/use-dashboard-stats";
import { Skeleton } from "@/components/ui/skeleton";

export default function TenantDashboardPage() {
    const { stats, isLoading } = useDashboardStats();

    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex flex-col gap-2">
                    <h1 className="text-3xl font-bold tracking-tight">Tenant Overview</h1>
                    <p className="text-muted-foreground">
                        Monitor your workspace activity and system performance.
                    </p>
                </div>

                {/* Stats Grid */}
                <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    <Card>
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">Workspace Users</CardTitle>
                            <Users className="size-4 text-muted-foreground" />
                        </CardHeader>
                        <CardContent>
                            {isLoading ? (
                                <Skeleton className="h-8 w-20" />
                            ) : (
                                <div className="text-2xl font-bold">{stats?.totalUsers ?? 0}</div>
                            )}
                            <p className="text-xs text-muted-foreground">Active members in your tenant</p>
                        </CardContent>
                    </Card>

                    <Card>
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">Recent Activities</CardTitle>
                            <Clock className="size-4 text-muted-foreground" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold">12</div>
                            <p className="text-xs text-muted-foreground">In the last 24 hours</p>
                        </CardContent>
                    </Card>

                    <Card>
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">API Health</CardTitle>
                            <CheckCircle2 className="size-4 text-emerald-500" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold text-emerald-600">Stable</div>
                            <p className="text-xs text-muted-foreground">Normal response times</p>
                        </CardContent>
                    </Card>

                    <Card>
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">System Status</CardTitle>
                            <Activity className="size-4 text-blue-500" />
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold">Operational</div>
                            <p className="text-xs text-muted-foreground">All systems go</p>
                        </CardContent>
                    </Card>
                </div>

                {/* Main Content */}
                <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-7">
                    {/* Activity Feed */}
                    <Card className="lg:col-span-3">
                        <CardHeader>
                            <CardTitle>Recent Activity</CardTitle>
                            <CardDescription>Latest changes in your workspace.</CardDescription>
                        </CardHeader>
                        <CardContent>
                            <ActivityFeed />
                        </CardContent>
                    </Card>

                    {/* API Performance */}
                    <Card className="lg:col-span-4">
                        <CardHeader>
                            <CardTitle>API Performance</CardTitle>
                            <CardDescription>Request volume and response trends.</CardDescription>
                        </CardHeader>
                        <CardContent className="pl-2">
                            <RequestsChart />
                        </CardContent>
                    </Card>

                    {/* System Health Detailed */}
                    <Card className="lg:col-span-7">
                        <CardHeader>
                            <CardTitle>System Health</CardTitle>
                            <CardDescription>Real-time status of critical infrastructure.</CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="grid gap-4 md:grid-cols-3">
                                <div className="flex items-center justify-between p-4 rounded-xl border bg-card/50">
                                    <div className="flex items-center gap-3">
                                        <div className="size-3 rounded-full bg-emerald-500" />
                                        <div>
                                            <p className="text-sm font-semibold">Web Server</p>
                                            <p className="text-xs text-muted-foreground">99.9% Uptime</p>
                                        </div>
                                    </div>
                                    <span className="text-[10px] bg-emerald-500/10 text-emerald-600 px-2 py-0.5 rounded-full font-bold uppercase">Online</span>
                                </div>
                                <div className="flex items-center justify-between p-4 rounded-xl border bg-card/50">
                                    <div className="flex items-center gap-3">
                                        <div className="size-3 rounded-full bg-emerald-500" />
                                        <div>
                                            <p className="text-sm font-semibold">Database</p>
                                            <p className="text-xs text-muted-foreground">Healthy Connectivity</p>
                                        </div>
                                    </div>
                                    <span className="text-[10px] bg-emerald-500/10 text-emerald-600 px-2 py-0.5 rounded-full font-bold uppercase">Healthy</span>
                                </div>
                                <div className="flex items-center justify-between p-4 rounded-xl border bg-card/50">
                                    <div className="flex items-center gap-3">
                                        <div className="size-3 rounded-full bg-amber-500" />
                                        <div>
                                            <p className="text-sm font-semibold">Background Jobs</p>
                                            <p className="text-xs text-muted-foreground">3 Jobs Processing</p>
                                        </div>
                                    </div>
                                    <span className="text-[10px] bg-amber-500/10 text-amber-600 px-2 py-0.5 rounded-full font-bold uppercase">Busy</span>
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </AppShell>
    );
}
