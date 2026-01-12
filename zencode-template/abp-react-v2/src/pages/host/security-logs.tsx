import { AppShell } from "@/components/layout/shell";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import {
    Search,
    Calendar as CalendarIcon,
    User,
    Globe
} from "lucide-react";

// Mock Data
const securityLogs = [
    {
        id: "1",
        time: "08/01/2026 08:58",
        action: "LoginSucceeded",
        ipAddress: "::1",
        browser: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)...",
        application: "Sapienza.Cursos.HttpApi.Host",
        identity: "Identity",
        user: "admin",
        client: "-",
        correlationId: "-"
    },
    {
        id: "2",
        time: "07/01/2026 21:41",
        action: "LoginSucceeded",
        ipAddress: "::1",
        browser: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)...",
        application: "Sapienza.Cursos.HttpApi.Host",
        identity: "Identity",
        user: "admin",
        client: "-",
        correlationId: "-"
    },
    {
        id: "3",
        time: "07/01/2026 21:39",
        action: "LoginFailed",
        ipAddress: "::1",
        browser: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)...",
        application: "Sapienza.Cursos.HttpApi.Host",
        identity: "Identity",
        user: "adminadmin",
        client: "-",
        correlationId: "-"
    },
    {
        id: "4",
        time: "07/01/2026 21:33",
        action: "LoginSucceeded",
        ipAddress: "::1",
        browser: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)...",
        application: "Sapienza.Cursos.HttpApi.Host",
        identity: "Identity",
        user: "admin",
        client: "-",
        correlationId: "-"
    }
];

export default function SecurityLogsPage() {
    return (
        <AppShell>
            <div className="space-y-6">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Logs de segurança</h1>
                </div>

                <Card className="border-none shadow-sm bg-card/60 backdrop-blur-sm">
                    <CardContent className="pt-6">
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                            <div className="space-y-2">
                                <Label>Hora de início</Label>
                                <div className="relative">
                                    <CalendarIcon className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                    <Input type="date" className="pl-10" />
                                </div>
                            </div>
                            <div className="space-y-2">
                                <Label>Hora de término</Label>
                                <div className="relative">
                                    <CalendarIcon className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                    <Input type="date" className="pl-10" />
                                </div>
                            </div>
                            <div className="space-y-2">
                                <Label>Aplicação</Label>
                                <Input placeholder="Aplicação..." />
                            </div>
                            <div className="space-y-2">
                                <Label>Identidade</Label>
                                <Input placeholder="Identidade..." />
                            </div>
                            <div className="space-y-2">
                                <Label>Nome do usuário</Label>
                                <div className="relative">
                                    <User className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                    <Input placeholder="Usuário..." className="pl-10" />
                                </div>
                            </div>
                            <div className="space-y-2">
                                <Label>Ação</Label>
                                <Input placeholder="Ação..." />
                            </div>
                            <div className="space-y-2">
                                <Label>Cliente</Label>
                                <Input placeholder="Cliente..." />
                            </div>
                            <div className="space-y-2">
                                <Label>Id de Correlação</Label>
                                <Input placeholder="ID..." />
                            </div>
                            <div className="space-y-2">
                                <Label>Endereço IP</Label>
                                <div className="relative">
                                    <Globe className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                    <Input placeholder="IP..." className="pl-10" />
                                </div>
                            </div>
                        </div>

                        <div className="flex justify-end gap-3 mt-6">
                            <Button variant="outline" className="px-12">Limpar</Button>
                            <Button className="px-12 gap-2">
                                <Search className="size-4" />
                                Procurar
                            </Button>
                        </div>
                    </CardContent>
                </Card>

                <Card className="border-none shadow-sm overflow-hidden">
                    <CardContent className="p-0">
                        <Table>
                            <TableHeader className="bg-muted/30">
                                <TableRow>
                                    <TableHead className="text-[10px] uppercase">Encontro</TableHead>
                                    <TableHead className="text-[10px] uppercase">Ação</TableHead>
                                    <TableHead className="text-[10px] uppercase">Endereço IP</TableHead>
                                    <TableHead className="text-[10px] uppercase">Navegador</TableHead>
                                    <TableHead className="text-[10px] uppercase">Aplicação</TableHead>
                                    <TableHead className="text-[10px] uppercase">Identidade</TableHead>
                                    <TableHead className="text-[10px] uppercase">Nome do Usuário</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {securityLogs.map((log) => (
                                    <TableRow key={log.id} className="text-xs">
                                        <TableCell>{log.time}</TableCell>
                                        <TableCell>
                                            <span className={log.action === "LoginFailed" ? "text-destructive font-medium" : "text-emerald-600 font-medium"}>
                                                {log.action}
                                            </span>
                                        </TableCell>
                                        <TableCell className="font-mono text-muted-foreground">{log.ipAddress}</TableCell>
                                        <TableCell className="max-w-[150px] truncate text-muted-foreground">{log.browser}</TableCell>
                                        <TableCell className="max-w-[150px] truncate">{log.application}</TableCell>
                                        <TableCell>{log.identity}</TableCell>
                                        <TableCell className="font-medium">{log.user}</TableCell>
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
