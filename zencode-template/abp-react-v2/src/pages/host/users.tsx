import { Shell } from "@/components/layout/shell";
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
import { Users, Search, Plus, Mail, Shield, MoreHorizontal } from "lucide-react";

// Mock data for users
const mockUsers = [
    {
        id: "1",
        userName: "admin",
        name: "Administrator",
        email: "admin@localhost",
        phoneNumber: null,
        isActive: true,
        roles: ["admin"],
        createdAt: "2024-01-01",
    },
    {
        id: "2",
        userName: "alex",
        name: "AlexSandro Cruz",
        email: "alex@sapienza.com.br",
        phoneNumber: "+55 11 99999-9999",
        isActive: true,
        roles: ["admin", "user"],
        createdAt: "2024-01-15",
    },
    {
        id: "3",
        userName: "joao.silva",
        name: "João Silva",
        email: "joao@educatec.com.br",
        phoneNumber: "+55 21 98888-8888",
        isActive: true,
        roles: ["user"],
        createdAt: "2024-02-20",
    },
    {
        id: "4",
        userName: "maria.santos",
        name: "Maria Santos",
        email: "maria@demo.com",
        phoneNumber: null,
        isActive: false,
        roles: ["user"],
        createdAt: "2024-03-10",
    },
];

export default function HostUsersPage() {
    return (
        <Shell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex items-center gap-3">
                    <Users className="h-8 w-8 text-primary" />
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Users</h1>
                        <p className="text-muted-foreground">
                            Manage users across all workspaces
                        </p>
                    </div>
                </div>

                {/* Users Table */}
                <Card>
                    <CardHeader>
                        <div className="flex items-center justify-between">
                            <CardTitle>Users ({mockUsers.length})</CardTitle>
                            <div className="flex items-center gap-4">
                                <div className="relative w-72">
                                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                    <Input
                                        placeholder="Search by name, email or username..."
                                        className="pl-10"
                                    />
                                </div>
                                <Button className="gap-2">
                                    <Plus className="size-4" />
                                    New User
                                </Button>
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>User</TableHead>
                                    <TableHead>Email</TableHead>
                                    <TableHead>Phone</TableHead>
                                    <TableHead>Roles</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead>Created</TableHead>
                                    <TableHead className="text-right">Actions</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {mockUsers.map((user) => (
                                    <TableRow key={user.id}>
                                        <TableCell>
                                            <div className="flex items-center gap-3">
                                                <div className="h-9 w-9 rounded-full bg-primary/10 flex items-center justify-center">
                                                    <span className="text-sm font-medium text-primary">
                                                        {user.name.split(' ').map(n => n[0]).join('').slice(0, 2)}
                                                    </span>
                                                </div>
                                                <div>
                                                    <p className="font-medium">{user.name}</p>
                                                    <p className="text-xs text-muted-foreground">@{user.userName}</p>
                                                </div>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-2">
                                                <Mail className="h-4 w-4 text-muted-foreground" />
                                                <span className="text-sm">{user.email}</span>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            {user.phoneNumber || <span className="text-muted-foreground">—</span>}
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex gap-1">
                                                {user.roles.map((role) => (
                                                    <Badge key={role} variant="secondary" className="gap-1">
                                                        <Shield className="h-3 w-3" />
                                                        {role}
                                                    </Badge>
                                                ))}
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <Badge variant={user.isActive ? "success" : "destructive"}>
                                                {user.isActive ? "Active" : "Inactive"}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>
                                            {new Date(user.createdAt).toLocaleDateString("pt-BR")}
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
            </div>
        </Shell>
    );
}
