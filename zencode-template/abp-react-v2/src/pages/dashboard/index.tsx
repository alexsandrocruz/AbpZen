import { Shell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Users, Building2, Shield, Activity } from "lucide-react";

const stats = [
    {
        title: "Total Users",
        value: "2,847",
        description: "+12% from last month",
        icon: Users,
        color: "text-blue-500",
        bgColor: "bg-blue-500/10",
    },
    {
        title: "Workspaces",
        value: "48",
        description: "+3 this week",
        icon: Building2,
        color: "text-emerald-500",
        bgColor: "bg-emerald-500/10",
    },
    {
        title: "Active Roles",
        value: "12",
        description: "System-wide",
        icon: Shield,
        color: "text-purple-500",
        bgColor: "bg-purple-500/10",
    },
    {
        title: "API Requests",
        value: "1.2M",
        description: "This month",
        icon: Activity,
        color: "text-amber-500",
        bgColor: "bg-amber-500/10",
    },
];

export default function DashboardPage() {
    return (
        <Shell>
            <div className="space-y-6">
                {/* Page Header */}
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
                    <p className="text-muted-foreground">
                        Welcome back! Here's an overview of your system.
                    </p>
                </div>

                {/* Stats Grid */}
                <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                    {stats.map((stat) => (
                        <Card key={stat.title}>
                            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                                <CardTitle className="text-sm font-medium">
                                    {stat.title}
                                </CardTitle>
                                <div className={`p-2 rounded-lg ${stat.bgColor}`}>
                                    <stat.icon className={`size-4 ${stat.color}`} />
                                </div>
                            </CardHeader>
                            <CardContent>
                                <div className="text-2xl font-bold">{stat.value}</div>
                                <p className="text-xs text-muted-foreground">
                                    {stat.description}
                                </p>
                            </CardContent>
                        </Card>
                    ))}
                </div>

                {/* Placeholder for more content */}
                <div className="grid gap-4 md:grid-cols-2">
                    <Card>
                        <CardHeader>
                            <CardTitle>Recent Activity</CardTitle>
                            <CardDescription>
                                Latest actions in your system
                            </CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="text-muted-foreground text-sm">
                                Activity feed coming soon...
                            </div>
                        </CardContent>
                    </Card>

                    <Card>
                        <CardHeader>
                            <CardTitle>Quick Actions</CardTitle>
                            <CardDescription>
                                Common administrative tasks
                            </CardDescription>
                        </CardHeader>
                        <CardContent>
                            <div className="text-muted-foreground text-sm">
                                Quick actions panel coming soon...
                            </div>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </Shell>
    );
}
