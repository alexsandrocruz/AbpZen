import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Building2, Search, Users, Plus, Settings, ExternalLink } from "lucide-react";

// Mock data for workspaces (will be replaced with API calls)
const mockWorkspaces = [
    {
        id: "1",
        name: "Sapienza ID",
        slug: "sapienza",
        ownerName: "AlexSandro Cruz",
        ownerEmail: "alex@sapienza.com.br",
        memberCount: 5,
        plan: "PROFESSIONAL",
        status: "ACTIVE",
        createdAt: "2024-01-15",
    },
    {
        id: "2",
        name: "Educatec",
        slug: "educatec",
        ownerName: "João Silva",
        ownerEmail: "joao@educatec.com.br",
        memberCount: 12,
        plan: "ENTERPRISE",
        status: "ACTIVE",
        createdAt: "2024-02-20",
    },
    {
        id: "3",
        name: "Demo Corp",
        slug: "demo",
        ownerName: "Maria Santos",
        ownerEmail: "maria@demo.com",
        memberCount: 3,
        plan: "TRIAL",
        status: "TRIAL",
        createdAt: "2024-03-10",
    },
];

const planColors: Record<string, "default" | "secondary" | "success" | "warning"> = {
    TRIAL: "secondary",
    STARTER: "default",
    PROFESSIONAL: "success",
    ENTERPRISE: "warning",
};

const statusColors: Record<string, "default" | "secondary" | "success" | "destructive"> = {
    ACTIVE: "success",
    TRIAL: "secondary",
    SUSPENDED: "destructive",
};

export default function HostWorkspacesPage() {
    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex items-center gap-3">
                    <Building2 className="h-8 w-8 text-primary" />
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Workspaces</h1>
                        <p className="text-muted-foreground">
                            Manage workspaces (tenants) in the platform
                        </p>
                    </div>
                </div>

                {/* Workspaces Table */}
                <Card>
                    <CardHeader>
                        <div className="flex items-center justify-between">
                            <CardTitle>Workspaces ({mockWorkspaces.length})</CardTitle>
                            <div className="flex items-center gap-4">
                                <div className="relative w-72">
                                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                    <Input
                                        placeholder="Search by name, slug or email..."
                                        className="pl-10"
                                    />
                                </div>
                                <Button className="gap-2">
                                    <Plus className="size-4" />
                                    New Workspace
                                </Button>
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Workspace</TableHead>
                                    <TableHead>Owner</TableHead>
                                    <TableHead className="text-center">Members</TableHead>
                                    <TableHead>Plan</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead>Created</TableHead>
                                    <TableHead className="text-right">Actions</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {mockWorkspaces.map((workspace) => (
                                    <TableRow key={workspace.id}>
                                        <TableCell>
                                            <div>
                                                <p className="font-medium">{workspace.name}</p>
                                                <p className="text-xs text-muted-foreground">/{workspace.slug}</p>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <div>
                                                <p className="text-sm">{workspace.ownerName}</p>
                                                <p className="text-xs text-muted-foreground">{workspace.ownerEmail}</p>
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-center">
                                            <div className="flex items-center justify-center gap-1">
                                                <Users className="h-4 w-4 text-muted-foreground" />
                                                <span>{workspace.memberCount}</span>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <Badge variant={planColors[workspace.plan] || "default"}>
                                                {workspace.plan}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>
                                            <Badge variant={statusColors[workspace.status] || "secondary"}>
                                                {workspace.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>
                                            {new Date(workspace.createdAt).toLocaleDateString("pt-BR")}
                                        </TableCell>
                                        <TableCell className="text-right">
                                            <div className="flex items-center justify-end gap-2">
                                                <Button variant="ghost" size="icon">
                                                    <Settings className="h-4 w-4" />
                                                </Button>
                                                <Button variant="ghost" size="icon">
                                                    <ExternalLink className="h-4 w-4" />
                                                </Button>
                                            </div>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>
            </div>
        </AppShell>
    );
}
