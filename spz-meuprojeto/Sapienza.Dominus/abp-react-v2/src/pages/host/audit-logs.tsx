import { useState } from "react";
import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";
import { Badge } from "@/components/ui/badge";
import {
    Search,
    Filter,
    Eye,
    Calendar as CalendarIcon,
    CheckCircle2,
    History,
    Database,
    User,
    Globe,
    Clock,
    AlertCircle
} from "lucide-react";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";

// Mock Data
const auditLogs = [
    {
        id: "1",
        httpStatus: 200,
        method: "POST",
        url: "/connect/token",
        user: "admin",
        ipAddress: "::1",
        time: "08/01/2026 12:21",
        duration: 89,
        appName: "Sapienza.Cursos.HttpApi.Host",
        browser: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)...",
        correlationId: "abc-123",
        hasException: false
    },
    {
        id: "2",
        httpStatus: 200,
        method: "GET",
        url: "/api/abp/application-configuration",
        user: "admin",
        ipAddress: "::1",
        time: "08/01/2026 12:13",
        duration: 133,
        appName: "Sapienza.Cursos.HttpApi.Host",
        browser: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)...",
        correlationId: "def-456",
        hasException: false
    },
    {
        id: "3",
        httpStatus: 500,
        method: "POST",
        url: "/api/account/register",
        user: "-",
        ipAddress: "192.168.1.10",
        time: "08/01/2026 11:58",
        duration: 212,
        appName: "Sapienza.Cursos.HttpApi.Host",
        browser: "Chrome/143.0.0.0",
        correlationId: "ghi-789",
        hasException: true,
        exception: "Internal Server Error: Database connection timeout."
    },
    {
        id: "4",
        httpStatus: 200,
        method: "GET",
        url: "/api/multi-tenancy/tenants/by-name/google",
        user: "alex",
        ipAddress: "::1",
        time: "08/01/2026 11:43",
        duration: 376,
        appName: "Sapienza.Cursos.HttpApi.Host",
        browser: "Edge/143.0.0.0",
        correlationId: "jkl-012",
        hasException: false
    }
];

const entityChanges = [
    {
        id: "101",
        time: "08/01/2026 12:25",
        changeType: "Atualizado",
        changeTypeVal: 1, // 0: Created, 1: Updated, 2: Deleted
        tenantId: "3a1b2c3d...",
        entityType: "Volo.Abp.Identity.IdentityUser",
        entityId: "u-999",
        properties: [
            { name: "Name", oldValue: "Alex", newValue: "Alex Cruz" },
            { name: "Surname", oldValue: "Silva", newValue: "Rocha" }
        ]
    },
    {
        id: "102",
        time: "08/01/2026 12:10",
        changeType: "Criado",
        changeTypeVal: 0,
        tenantId: "3a1b2c3d...",
        entityType: "Volo.Abp.TenantManagement.Tenant",
        entityId: "t-777",
        properties: [
            { name: "Name", oldValue: null, newValue: "ZenCode V2" }
        ]
    },
    {
        id: "103",
        time: "08/01/2026 11:30",
        changeType: "Excluido",
        changeTypeVal: 2,
        tenantId: "default",
        entityType: "Volo.Abp.Identity.IdentityRole",
        entityId: "r-555",
        properties: []
    }
];

export default function AuditLogsPage() {
    const [selectedLog, setSelectedLog] = useState<any>(null);
    const [selectedEntityChange, setSelectedEntityChange] = useState<any>(null);
    const [isDetailOpen, setIsDetailOpen] = useState(false);
    const [isEntityDetailOpen, setIsEntityDetailOpen] = useState(false);

    const getMethodColor = (method: string) => {
        switch (method) {
            case "GET": return "bg-blue-500/10 text-blue-500 border-blue-500/20";
            case "POST": return "bg-emerald-500/10 text-emerald-500 border-emerald-500/20";
            case "PUT": return "bg-amber-500/10 text-amber-500 border-amber-500/20";
            case "DELETE": return "bg-rose-500/10 text-rose-500 border-rose-500/20";
            default: return "bg-slate-500/10 text-slate-500 border-slate-500/20";
        }
    };

    const getChangeTypeColor = (type: number) => {
        switch (type) {
            case 0: return "bg-emerald-500/10 text-emerald-500 border-emerald-500/20"; // Created
            case 1: return "bg-blue-500/10 text-blue-500 border-blue-500/20";       // Updated
            case 2: return "bg-rose-500/10 text-rose-500 border-rose-500/20";       // Deleted
            default: return "bg-slate-500/10 text-slate-500 border-slate-500/20";
        }
    };

    return (
        <AppShell>
            <div className="space-y-6">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Logs de auditoria</h1>
                    <p className="text-muted-foreground">Monitore o histórico de acessos e mudanças no sistema.</p>
                </div>

                <Tabs defaultValue="audit" className="w-full">
                    <TabsList className="bg-muted/50 p-1 mb-6">
                        <TabsTrigger value="audit" className="px-6">Logs de auditoria</TabsTrigger>
                        <TabsTrigger value="entity" className="px-6">Mudanças da Entidade</TabsTrigger>
                    </TabsList>

                    <TabsContent value="audit" className="space-y-6">
                        {/* Filters Card */}
                        <Card className="border-none shadow-sm bg-card/60 backdrop-blur-sm">
                            <CardHeader className="pb-4">
                                <div className="flex items-center gap-2">
                                    <Filter className="size-4 text-primary" />
                                    <CardTitle className="text-lg">Filtros Avançados</CardTitle>
                                </div>
                            </CardHeader>
                            <CardContent>
                                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                                    <div className="space-y-2">
                                        <Label>Data inicial</Label>
                                        <div className="relative">
                                            <CalendarIcon className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                            <Input type="date" className="pl-10" />
                                        </div>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Data final</Label>
                                        <div className="relative">
                                            <CalendarIcon className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                            <Input type="date" className="pl-10" />
                                        </div>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Nome do usuário</Label>
                                        <div className="relative">
                                            <User className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                            <Input placeholder="admin..." className="pl-10" />
                                        </div>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Url</Label>
                                        <div className="relative">
                                            <Globe className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                            <Input placeholder="/api/..." className="pl-10" />
                                        </div>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Método Http</Label>
                                        <Select>
                                            <SelectTrigger>
                                                <SelectValue placeholder="-" />
                                            </SelectTrigger>
                                            <SelectContent>
                                                <SelectItem value="get">GET</SelectItem>
                                                <SelectItem value="post">POST</SelectItem>
                                                <SelectItem value="put">PUT</SelectItem>
                                                <SelectItem value="delete">DELETE</SelectItem>
                                            </SelectContent>
                                        </Select>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Código de status</Label>
                                        <Select>
                                            <SelectTrigger>
                                                <SelectValue placeholder="-" />
                                            </SelectTrigger>
                                            <SelectContent>
                                                <SelectItem value="200">200 OK</SelectItem>
                                                <SelectItem value="400">400 Bad Request</SelectItem>
                                                <SelectItem value="401">401 Unauthorized</SelectItem>
                                                <SelectItem value="403">403 Forbidden</SelectItem>
                                                <SelectItem value="404">404 Not Found</SelectItem>
                                                <SelectItem value="500">500 Internal Error</SelectItem>
                                            </SelectContent>
                                        </Select>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Tem exceção</Label>
                                        <Select>
                                            <SelectTrigger>
                                                <SelectValue placeholder="-" />
                                            </SelectTrigger>
                                            <SelectContent>
                                                <SelectItem value="yes">Sim</SelectItem>
                                                <SelectItem value="no">Não</SelectItem>
                                            </SelectContent>
                                        </Select>
                                    </div>
                                    <div className="flex items-end">
                                        <Button className="w-full gap-2 shadow-lg shadow-primary/20">
                                            <Search className="size-4" />
                                            Atualizar
                                        </Button>
                                    </div>
                                </div>
                            </CardContent>
                        </Card>

                        {/* Results Table */}
                        <Card className="border-none shadow-sm overflow-hidden">
                            <CardContent className="p-0">
                                <Table>
                                    <TableHeader className="bg-muted/30">
                                        <TableRow>
                                            <TableHead className="w-[100px]">Ações</TableHead>
                                            <TableHead>Pedido HTTP</TableHead>
                                            <TableHead>Usuário</TableHead>
                                            <TableHead>Endereço IP</TableHead>
                                            <TableHead>Encontro</TableHead>
                                            <TableHead>Duração</TableHead>
                                            <TableHead className="hidden lg:table-cell">Aplicação</TableHead>
                                        </TableRow>
                                    </TableHeader>
                                    <TableBody>
                                        {auditLogs.map((log) => (
                                            <TableRow key={log.id} className="group transition-colors hover:bg-muted/20">
                                                <TableCell>
                                                    <Button
                                                        variant="ghost"
                                                        size="sm"
                                                        className="h-8 gap-2 hover:bg-primary/10 hover:text-primary"
                                                        onClick={() => {
                                                            setSelectedLog(log);
                                                            setIsDetailOpen(true);
                                                        }}
                                                    >
                                                        <Eye className="size-3.5" />
                                                        Detalhes
                                                    </Button>
                                                </TableCell>
                                                <TableCell>
                                                    <div className="flex items-center gap-2">
                                                        <Badge
                                                            variant={log.httpStatus >= 400 ? "destructive" : "outline"}
                                                            className="font-mono text-[10px] h-5"
                                                        >
                                                            {log.httpStatus}
                                                        </Badge>
                                                        <Badge
                                                            className={`font-mono text-[10px] h-5 border ${getMethodColor(log.method)}`}
                                                        >
                                                            {log.method}
                                                        </Badge>
                                                        <span className="text-sm font-medium truncate max-w-[200px] text-muted-foreground group-hover:text-foreground transition-colors">
                                                            {log.url}
                                                        </span>
                                                        {log.hasException && (
                                                            <AlertCircle className="size-3.5 text-destructive" />
                                                        )}
                                                    </div>
                                                </TableCell>
                                                <TableCell className="text-sm">{log.user}</TableCell>
                                                <TableCell className="text-sm font-mono text-muted-foreground">{log.ipAddress}</TableCell>
                                                <TableCell className="text-sm">{log.time}</TableCell>
                                                <TableCell>
                                                    <div className="flex items-center gap-1.5 text-sm">
                                                        <Clock className="size-3.5 text-muted-foreground" />
                                                        {log.duration}ms
                                                    </div>
                                                </TableCell>
                                                <TableCell className="hidden lg:table-cell">
                                                    <div className="text-xs text-muted-foreground truncate max-w-[150px]">
                                                        {log.appName}
                                                    </div>
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </CardContent>
                        </Card>
                    </TabsContent>

                    <TabsContent value="entity" className="space-y-6">
                        {/* Entity Filters Card */}
                        <Card className="border-none shadow-sm bg-card/60 backdrop-blur-sm">
                            <CardHeader className="pb-4">
                                <div className="flex items-center gap-2">
                                    <Filter className="size-4 text-primary" />
                                    <CardTitle className="text-lg">Filtros de Mudança</CardTitle>
                                </div>
                            </CardHeader>
                            <CardContent>
                                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                                    <div className="space-y-2">
                                        <Label>Tempo</Label>
                                        <div className="relative">
                                            <CalendarIcon className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                            <Input type="date" className="pl-10" />
                                        </div>
                                    </div>
                                    <div className="space-y-2">
                                        <Label>Tipo de mudança</Label>
                                        <Select>
                                            <SelectTrigger>
                                                <SelectValue placeholder="-" />
                                            </SelectTrigger>
                                            <SelectContent>
                                                <SelectItem value="0">Criado</SelectItem>
                                                <SelectItem value="1">Atualizado</SelectItem>
                                                <SelectItem value="2">Excluido</SelectItem>
                                            </SelectContent>
                                        </Select>
                                    </div>
                                    <div className="space-y-2 lg:col-span-2">
                                        <Label>Nome completo do tipo da entidade</Label>
                                        <div className="relative">
                                            <Database className="absolute left-3 top-2.5 size-4 text-muted-foreground" />
                                            <Input placeholder="Volo.Abp.Identity..." className="pl-10" />
                                        </div>
                                    </div>
                                    <div className="lg:col-start-4">
                                        <Button className="w-full gap-2 shadow-lg shadow-primary/20">
                                            <Search className="size-4" />
                                            Atualizar
                                        </Button>
                                    </div>
                                </div>
                            </CardContent>
                        </Card>

                        {/* Entity Results Table */}
                        <Card className="border-none shadow-sm overflow-hidden">
                            <CardContent className="p-0">
                                <Table>
                                    <TableHeader className="bg-muted/30">
                                        <TableRow>
                                            <TableHead className="w-[100px]">Detalhe</TableHead>
                                            <TableHead>Tempo</TableHead>
                                            <TableHead>Tipo de Mudança</TableHead>
                                            <TableHead>TenantId</TableHead>
                                            <TableHead>Nome completo do tipo da entidade</TableHead>
                                        </TableRow>
                                    </TableHeader>
                                    <TableBody>
                                        {entityChanges.map((change) => (
                                            <TableRow key={change.id} className="group transition-colors hover:bg-muted/20">
                                                <TableCell>
                                                    <Button
                                                        variant="ghost"
                                                        size="sm"
                                                        className="h-8 gap-2 hover:bg-primary/10 hover:text-primary"
                                                        onClick={() => {
                                                            setSelectedEntityChange(change);
                                                            setIsEntityDetailOpen(true);
                                                        }}
                                                    >
                                                        <Eye className="size-3.5" />
                                                        Detalhes
                                                    </Button>
                                                </TableCell>
                                                <TableCell className="text-sm">
                                                    <div className="flex items-center gap-1.5">
                                                        <Clock className="size-3.5 text-muted-foreground" />
                                                        {change.time}
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <Badge className={`font-mono text-[10px] h-5 border ${getChangeTypeColor(change.changeTypeVal)}`}>
                                                        {change.changeType}
                                                    </Badge>
                                                </TableCell>
                                                <TableCell className="text-sm font-mono text-muted-foreground max-w-[120px] truncate">
                                                    {change.tenantId}
                                                </TableCell>
                                                <TableCell>
                                                    <div className="text-sm font-medium text-foreground truncate max-w-[300px]">
                                                        {change.entityType}
                                                    </div>
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                                <div className="p-4 border-t bg-muted/5">
                                    <p className="text-xs text-muted-foreground font-medium">
                                        {entityChanges.length} total
                                    </p>
                                </div>
                            </CardContent>
                        </Card>
                    </TabsContent>
                </Tabs>
            </div>

            {/* Detail Modal */}
            <Dialog open={isDetailOpen} onOpenChange={setIsDetailOpen}>
                <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
                    <DialogHeader>
                        <DialogTitle className="flex items-center gap-2 text-2xl">
                            Detalhes do Log
                        </DialogTitle>
                        <DialogDescription>
                            Informações completas sobre a execução técnica deste pedido.
                        </DialogDescription>
                    </DialogHeader>

                    {selectedLog && (
                        <Tabs defaultValue="general" className="w-full mt-4">
                            <TabsList className="bg-muted/50 w-full justify-start p-1 h-auto gap-1">
                                <TabsTrigger value="general" className="px-8 py-2">No geral</TabsTrigger>
                                <TabsTrigger value="actions" className="px-8 py-2">Ações (1)</TabsTrigger>
                            </TabsList>

                            <TabsContent value="general" className="mt-6">
                                <div className="space-y-0 text-sm border rounded-lg overflow-hidden divide-y">
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Código de status Http</div>
                                        <div className="w-2/3 flex items-center gap-2">
                                            <Badge className={selectedLog.httpStatus >= 400 ? "bg-rose-500" : "bg-emerald-500"}>
                                                {selectedLog.httpStatus}
                                            </Badge>
                                            {selectedLog.httpStatus === 200 && <CheckCircle2 className="size-4 text-emerald-500" />}
                                        </div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Método Http</div>
                                        <div className="w-2/3">
                                            <Badge className={getMethodColor(selectedLog.method)}>{selectedLog.method}</Badge>
                                        </div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Url</div>
                                        <div className="w-2/3 font-mono text-[13px] text-primary">{selectedLog.url}</div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Endereço IP do cliente</div>
                                        <div className="w-2/3 font-mono">{selectedLog.ipAddress}</div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Nome do usuário</div>
                                        <div className="w-2/3">{selectedLog.user}</div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Tempo</div>
                                        <div className="w-2/3">{selectedLog.time}</div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Duração</div>
                                        <div className="w-2/3 flex items-center gap-1.5">
                                            <Clock className="size-4 text-muted-foreground" />
                                            {selectedLog.duration}ms
                                        </div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Informação do navegador</div>
                                        <div className="w-2/3 text-xs opacity-80 italic">{selectedLog.browser}</div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Nome da Aplicação</div>
                                        <div className="w-2/3 text-xs">{selectedLog.appName}</div>
                                    </div>
                                    <div className="flex p-3 hover:bg-muted/20 transition-colors">
                                        <div className="w-1/3 font-semibold text-muted-foreground">Id de Correlação</div>
                                        <div className="w-2/3 font-mono text-xs">{selectedLog.correlationId}</div>
                                    </div>
                                    {selectedLog.hasException && (
                                        <div className="flex p-3 bg-rose-500/5 text-rose-600">
                                            <div className="w-1/3 font-semibold flex items-center gap-2">
                                                <AlertCircle className="size-4" />
                                                Exceção
                                            </div>
                                            <div className="w-2/3 text-xs font-mono">{selectedLog.exception}</div>
                                        </div>
                                    )}
                                </div>
                            </TabsContent>

                            <TabsContent value="actions" className="mt-6">
                                <div className="p-8 text-center bg-muted/20 rounded-lg border border-dashed">
                                    <p className="text-sm text-muted-foreground">
                                        Lista de ações internas disparadas por este pedido será exibida aqui.
                                    </p>
                                </div>
                            </TabsContent>
                        </Tabs>
                    )}

                    <DialogFooter className="mt-6">
                        <Button variant="outline" onClick={() => setIsDetailOpen(false)}>Fechar</Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
            {/* Entity Change Detail Modal */}
            <Dialog open={isEntityDetailOpen} onOpenChange={setIsEntityDetailOpen}>
                <DialogContent className="max-w-4xl max-h-[90vh] overflow-y-auto">
                    <DialogHeader>
                        <DialogTitle className="flex items-center gap-2 text-2xl">
                            Detalhes da Mudança de Entidade
                        </DialogTitle>
                        <DialogDescription>
                            Análise granular das propriedades modificadas nesta operação.
                        </DialogDescription>
                    </DialogHeader>

                    {selectedEntityChange && (
                        <div className="mt-4 space-y-6">
                            <div className="grid grid-cols-2 gap-4 p-4 rounded-lg bg-muted/30 border">
                                <div className="space-y-1">
                                    <p className="text-xs text-muted-foreground uppercase font-bold tracking-wider">Entidade</p>
                                    <p className="text-sm font-medium">{selectedEntityChange.entityType}</p>
                                </div>
                                <div className="space-y-1">
                                    <p className="text-xs text-muted-foreground uppercase font-bold tracking-wider">ID da Entidade</p>
                                    <p className="text-sm font-mono">{selectedEntityChange.entityId}</p>
                                </div>
                                <div className="space-y-1">
                                    <p className="text-xs text-muted-foreground uppercase font-bold tracking-wider">Tempo</p>
                                    <p className="text-sm">{selectedEntityChange.time}</p>
                                </div>
                                <div className="space-y-1">
                                    <p className="text-xs text-muted-foreground uppercase font-bold tracking-wider">Tipo da Mudança</p>
                                    <Badge className={getChangeTypeColor(selectedEntityChange.changeTypeVal)}>{selectedEntityChange.changeType}</Badge>
                                </div>
                            </div>

                            <div className="space-y-4">
                                <h4 className="font-semibold flex items-center gap-2">
                                    <History className="size-4 text-primary" />
                                    Propriedades Alteradas
                                </h4>

                                <div className="border rounded-lg overflow-hidden">
                                    <Table>
                                        <TableHeader className="bg-muted/50">
                                            <TableRow>
                                                <TableHead>Propriedade</TableHead>
                                                <TableHead>Valor Antigo</TableHead>
                                                <TableHead>Novo Valor</TableHead>
                                            </TableRow>
                                        </TableHeader>
                                        <TableBody>
                                            {selectedEntityChange.properties.length > 0 ? (
                                                selectedEntityChange.properties.map((prop: any) => (
                                                    <TableRow key={prop.name}>
                                                        <TableCell className="font-medium text-sm">{prop.name}</TableCell>
                                                        <TableCell className="text-sm text-muted-foreground line-through decoration-rose-500/50">
                                                            {prop.oldValue === null ? "null" : String(prop.oldValue)}
                                                        </TableCell>
                                                        <TableCell className="text-sm text-emerald-600 font-medium">
                                                            {prop.newValue === null ? "null" : String(prop.newValue)}
                                                        </TableCell>
                                                    </TableRow>
                                                ))
                                            ) : (
                                                <TableRow>
                                                    <TableCell colSpan={3} className="text-center py-8 text-muted-foreground italic">
                                                        Nenhuma propriedade alterada para exibir.
                                                    </TableCell>
                                                </TableRow>
                                            )}
                                        </TableBody>
                                    </Table>
                                </div>
                            </div>
                        </div>
                    )}

                    <DialogFooter className="mt-6">
                        <Button variant="outline" onClick={() => setIsEntityDetailOpen(false)}>Fechar</Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </AppShell>
    );
}
