import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import {
    Monitor,
    Smartphone,
    Settings2,
    MoreHorizontal,
    Clock,
    Globe,
    Info,
    Layout,
    UserCircle,
    ShieldCheck,
    LogOut
} from "lucide-react";
import { useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter
} from "@/components/ui/dialog";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { toast } from "sonner";
import { useAuth } from "@/lib/abp/auth";

// Mock Data for User Sessions
const mockSessions = [
    {
        id: "1",
        device: "MacBook Pro",
        deviceInfo: "macOS - Chrome 120.0.0",
        signedInDate: "08/01/2026 08:58",
        lastAccessDate: "08/01/2026 16:05",
        ip: "192.168.1.15",
        clientId: "AbpReact_App",
        userInfo: "admin (AlexSandro Cruz)",
        isCurrent: true
    },
    {
        id: "2",
        device: "iPhone 15 Pro",
        deviceInfo: "iOS 17.2 - Safari",
        signedInDate: "07/01/2026 14:22",
        lastAccessDate: "07/01/2026 14:30",
        ip: "177.45.12.89",
        clientId: "AbpReact_Mobile",
        userInfo: "admin (AlexSandro Cruz)",
        isCurrent: false
    }
];

export default function UserSessionsPage() {
    const { user } = useAuth();
    const [sessions, setSessions] = useState(mockSessions);
    const [selectedSession, setSelectedSession] = useState<any>(null);
    const [isDetailOpen, setIsDetailOpen] = useState(false);

    const handleOpenDetail = (session: any) => {
        setSelectedSession(session);
        setIsDetailOpen(true);
    };

    const handleRevoke = (id: string) => {
        setSessions(prev => prev.filter(s => s.id !== id));
        toast.success("Sessão revogada com sucesso!");
    };

    const handleRevokeAll = () => {
        setSessions(prev => prev.filter(s => s.isCurrent));
        toast.success("Todas as outras sessões foram revogadas!");
    };

    return (
        <AppShell>
            <div className="max-w-6xl mx-auto space-y-6">
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Suas Sessões</h1>
                        <p className="text-muted-foreground">Gerencie os dispositivos e navegadores onde você está conectado.</p>
                    </div>
                    <Button
                        variant="destructive"
                        className="font-bold gap-2 shadow-lg shadow-rose-500/20"
                        onClick={handleRevokeAll}
                    >
                        <LogOut size={18} />
                        Encerrar todas as outras sessões
                    </Button>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <Card className="md:col-span-1 border-none shadow-xl bg-gradient-to-br from-primary/10 via-background to-background backdrop-blur-sm h-fit">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <ShieldCheck className="text-primary size-5" />
                                Segurança da Conta
                            </CardTitle>
                            <CardDescription>
                                Monitoramento básico de acessos para <strong>{user?.userName}</strong>.
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="p-4 rounded-xl bg-background/50 border border-primary/10 space-y-1">
                                <p className="text-[10px] font-bold text-muted-foreground uppercase">Sessões Ativas</p>
                                <p className="text-2xl font-bold text-primary">{sessions.length}</p>
                            </div>
                            <div className="flex items-start gap-3 p-3 text-xs text-muted-foreground bg-muted/30 rounded-lg">
                                <Info size={14} className="shrink-0 mt-0.5" />
                                <p>Se você notar algum acesso suspeito, revogue a sessão imediatamente e altere sua senha.</p>
                            </div>
                        </CardContent>
                    </Card>

                    <Card className="md:col-span-2 border-none shadow-xl overflow-hidden">
                        <CardContent className="p-0">
                            <Table>
                                <TableHeader className="bg-muted/30">
                                    <TableRow>
                                        <TableHead className="w-[100px] font-bold text-[10px] uppercase pl-6">AÇÕES</TableHead>
                                        <TableHead className="font-bold text-[10px] uppercase">DEVICE</TableHead>
                                        <TableHead className="font-bold text-[10px] uppercase">INFO</TableHead>
                                        <TableHead className="font-bold text-[10px] uppercase hidden sm:table-cell">SIGNED IN</TableHead>
                                        <TableHead className="font-bold text-[10px] uppercase pr-6">STATUS</TableHead>
                                    </TableRow>
                                </TableHeader>
                                <TableBody>
                                    {sessions.map((session) => (
                                        <TableRow key={session.id} className="group hover:bg-muted/5 transition-colors">
                                            <TableCell className="pl-6">
                                                <DropdownMenu>
                                                    <DropdownMenuTrigger asChild>
                                                        <Button variant="ghost" size="icon" className="h-8 w-8 hover:bg-primary/10 hover:text-primary transition-all">
                                                            <MoreHorizontal className="size-4" />
                                                        </Button>
                                                    </DropdownMenuTrigger>
                                                    <DropdownMenuContent align="start" className="w-48 p-2">
                                                        <DropdownMenuItem
                                                            className="gap-3 cursor-pointer py-2.5 rounded-lg"
                                                            onClick={() => handleOpenDetail(session)}
                                                        >
                                                            <Settings2 className="size-4 text-muted-foreground" />
                                                            <span className="font-medium">Ver Detalhes</span>
                                                        </DropdownMenuItem>
                                                        {!session.isCurrent && (
                                                            <DropdownMenuItem
                                                                className="gap-3 cursor-pointer py-2.5 rounded-lg text-destructive focus:text-destructive focus:bg-destructive/5"
                                                                onClick={() => handleRevoke(session.id)}
                                                            >
                                                                <LogOut className="size-4" />
                                                                <span className="font-medium">Encerrar Sessão</span>
                                                            </DropdownMenuItem>
                                                        )}
                                                    </DropdownMenuContent>
                                                </DropdownMenu>
                                            </TableCell>
                                            <TableCell className="font-medium">
                                                <div className="flex items-center gap-3">
                                                    {session.deviceInfo.toLowerCase().includes("macos") || session.deviceInfo.toLowerCase().includes("windows") ? (
                                                        <Monitor className="size-4 text-primary" />
                                                    ) : (
                                                        <Smartphone className="size-4 text-primary" />
                                                    )}
                                                    {session.device}
                                                </div>
                                            </TableCell>
                                            <TableCell className="text-xs text-muted-foreground whitespace-nowrap">
                                                {session.deviceInfo}
                                            </TableCell>
                                            <TableCell className="text-xs text-muted-foreground hidden sm:table-cell whitespace-nowrap">
                                                {session.signedInDate}
                                            </TableCell>
                                            <TableCell className="pr-6">
                                                {session.isCurrent ? (
                                                    <span className="inline-flex items-center px-2 py-0.5 rounded text-[10px] font-bold bg-emerald-500/10 text-emerald-600 border border-emerald-500/20">
                                                        ATUAL
                                                    </span>
                                                ) : (
                                                    <span className="inline-flex items-center px-2 py-0.5 rounded text-[10px] font-bold bg-muted text-muted-foreground border">
                                                        ATIVA
                                                    </span>
                                                )}
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </CardContent>
                    </Card>
                </div>

                {/* Session Detail Modal */}
                <Dialog open={isDetailOpen} onOpenChange={setIsDetailOpen}>
                    <DialogContent className="max-w-xl p-0 overflow-hidden border-none shadow-3xl">
                        <DialogHeader className="p-6 bg-primary/5 border-b">
                            <DialogTitle className="text-lg font-bold flex items-center gap-2">
                                <Info className="size-5 text-primary" />
                                Detalhes da Sessão
                            </DialogTitle>
                        </DialogHeader>

                        <div className="p-6 flex flex-col gap-0">
                            {[
                                { label: "Dispositivo", value: selectedSession?.device, icon: <Monitor className="size-4" /> },
                                { label: "Informações", value: selectedSession?.deviceInfo, icon: <Layout className="size-4" /> },
                                { label: "Usuário", value: selectedSession?.userInfo, icon: <UserCircle className="size-4" /> },
                                { label: "Client ID", value: selectedSession?.clientId, icon: <ShieldCheck className="size-4" /> },
                                { label: "Endereço IP", value: selectedSession?.ip, icon: <Globe className="size-4" /> },
                                { label: "Data de Início", value: selectedSession?.signedInDate, icon: <Clock className="size-4" /> },
                                { label: "Último Acesso", value: selectedSession?.lastAccessDate, icon: <Clock className="size-4" /> },
                            ].map((row, i) => (
                                <div key={i} className="flex items-center py-4 border-b last:border-0 border-muted/20 animate-in fade-in slide-in-from-left-2 duration-300" style={{ animationDelay: `${i * 50}ms` }}>
                                    <div className="w-1/3 flex items-center gap-2 text-sm font-bold text-muted-foreground uppercase tracking-tight">
                                        <div className="text-primary/60">{row.icon}</div>
                                        {row.label}
                                    </div>
                                    <div className="flex-1 text-sm font-semibold text-foreground">{row.value || "-"}</div>
                                </div>
                            ))}
                        </div>

                        <DialogFooter className="p-6 bg-muted/10 border-t">
                            <Button
                                className="px-10 font-bold shadow-lg shadow-primary/10"
                                onClick={() => setIsDetailOpen(false)}
                            >
                                Fechar
                            </Button>
                        </DialogFooter>
                    </DialogContent>
                </Dialog>
            </div>
        </AppShell>
    );
}
