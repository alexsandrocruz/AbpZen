import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
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
import { Shield, Search, Plus, Users, Check, MoreHorizontal } from "lucide-react";

// Mock data for roles
const mockRoles = [
    {
        id: "1",
        name: "admin",
        displayName: "Administrator",
        isDefault: false,
        isStatic: true,
        userCount: 2,
        permissions: ["AbpIdentity.Users", "AbpIdentity.Roles", "AbpTenantManagement.Tenants"],
    },
    {
        id: "2",
        name: "user",
        displayName: "Standard User",
        isDefault: true,
        isStatic: false,
        userCount: 15,
        permissions: ["AbpIdentity.Users.Read"],
    },
    {
        id: "3",
        name: "manager",
        displayName: "Manager",
        isDefault: false,
        isStatic: false,
        userCount: 5,
        permissions: ["AbpIdentity.Users", "AbpIdentity.Users.Create", "AbpIdentity.Users.Update"],
    },
];

export default function HostRolesPage() {
    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex items-center gap-3">
                    <Shield className="h-8 w-8 text-primary" />
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Roles</h1>
                        <p className="text-muted-foreground">
                            Manage roles and permissions
                        </p>
                    </div>
                </div>

                {/* Roles Table */}
                <Card>
                    <CardHeader>
                        <div className="flex items-center justify-between">
                            <CardTitle>Roles ({mockRoles.length})</CardTitle>
                            <div className="flex items-center gap-4">
                                <div className="relative w-72">
                                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                    <Input
                                        placeholder="Search roles..."
                                        className="pl-10"
                                    />
                                </div>
                                <Button className="gap-2">
                                    <Plus className="size-4" />
                                    New Role
                                </Button>
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Role</TableHead>
                                    <TableHead>Properties</TableHead>
                                    <TableHead className="text-center">Users</TableHead>
                                    <TableHead>Permissions</TableHead>
                                    <TableHead className="text-right">Actions</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {mockRoles.map((role) => (
                                    <TableRow key={role.id}>
                                        <TableCell>
                                            <div>
                                                <p className="font-medium">{role.displayName}</p>
                                                <p className="text-xs text-muted-foreground">{role.name}</p>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex gap-1">
                                                {role.isDefault && (
                                                    <Badge variant="secondary" className="gap-1">
                                                        <Check className="h-3 w-3" />
                                                        Default
                                                    </Badge>
                                                )}
                                                {role.isStatic && (
                                                    <Badge variant="outline">Static</Badge>
                                                )}
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-center">
                                            <div className="flex items-center justify-center gap-1">
                                                <Users className="h-4 w-4 text-muted-foreground" />
                                                <span>{role.userCount}</span>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-1">
                                                <Badge variant="secondary">
                                                    {role.permissions.length} permissions
                                                </Badge>
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-right">
                                            <Button variant="ghost" size="icon">
                                                <MoreHorizontal className="h-4 w-4" />
                                            </Button>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>

                {/* Permission Groups */}
                <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
                    {["Identity Management", "Tenant Management", "Settings"].map((group) => (
                        <Card key={group}>
                            <CardHeader className="pb-3">
                                <CardTitle className="text-base">{group}</CardTitle>
                                <CardDescription>
                                    Manage {group.toLowerCase()} permissions
                                </CardDescription>
                            </CardHeader>
                            <CardContent>
                                <div className="text-sm text-muted-foreground">
                                    Click on a role to configure permissions
                                </div>
                            </CardContent>
                        </Card>
                    ))}
                </div>
            </div>
        </AppShell>
    );
}
