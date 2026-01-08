import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
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
    ShieldCheck,
    Download,
    Trash2,
    AlertTriangle,
    Info,
    Clock,
    CheckCircle2,
    History
} from "lucide-react";
import { useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogTitle,
    DialogDescription
} from "@/components/ui/dialog";
import { toast } from "sonner";

// Mock Data for Personal Data Requests
const mockRequests = [
    {
        id: "1",
        action: "Exportação de Dados",
        status: "PREPARANDO",
        createdAt: "08/01/2026 16:04",
        readyAt: "08/01/2026 17:04",
    }
];

export default function LgpdPage() {
    const [requests, setRequests] = useState(mockRequests);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isRequesting, setIsRequesting] = useState(false);

    const handleRequestData = () => {
        setIsRequesting(true);
        setTimeout(() => {
            const newRequest = {
                id: (requests.length + 1).toString(),
                action: "Exportação de Dados",
                status: "PREPARANDO",
                createdAt: new Date().toLocaleString('pt-BR'),
                readyAt: "-",
            };
            setRequests([newRequest, ...requests]);
            setIsRequesting(false);
            toast.success("Solicitação enviada com sucesso! Você receberá um e-mail quando os dados estiverem prontos.");
        }, 1500);
    };

    const handleDeleteAccount = () => {
        toast.error("Processo de exclusão de conta iniciado. Você receberá um e-mail de confirmação.");
        setIsDeleteModalOpen(false);
    };

    return (
        <AppShell>
            <div className="max-w-6xl mx-auto space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Dados pessoais</h1>
                        <p className="text-muted-foreground italic">Gerencie sua privacidade e controle seus dados em conformidade com a LGPD.</p>
                    </div>
                    <div className="flex items-center gap-3">
                        <Button
                            className="font-bold gap-2 bg-indigo-600 hover:bg-indigo-700 shadow-lg shadow-indigo-500/20 h-10 px-6"
                            onClick={handleRequestData}
                            disabled={isRequesting}
                        >
                            {isRequesting ? <Clock className="animate-spin size-4" /> : <Download size={18} />}
                            Solicitar Dados Pessoais
                        </Button>
                        <Button
                            variant="destructive"
                            className="font-bold gap-2 shadow-lg shadow-rose-500/20 h-10 px-6"
                            onClick={() => setIsDeleteModalOpen(true)}
                        >
                            <Trash2 size={18} />
                            Excluir dados pessoais
                        </Button>
                    </div>
                </div>

                <div className="bg-blue-500/10 border border-blue-500/20 p-5 rounded-2xl flex gap-4 text-blue-700 dark:text-blue-400">
                    <Info className="size-6 shrink-0 mt-0.5" />
                    <p className="text-sm font-medium leading-relaxed">
                        Sua conta contém os dados pessoais que você nos forneceu. Esta página permite que você baixe ou exclua esses dados.
                    </p>
                </div>

                <Card className="border-none shadow-xl overflow-hidden">
                    <CardHeader className="bg-muted/30 border-b py-4">
                        <div className="flex items-center gap-2">
                            <History size={18} className="text-muted-foreground" />
                            <CardTitle className="text-sm font-bold uppercase tracking-wider">Histórico de Solicitações</CardTitle>
                        </div>
                    </CardHeader>
                    <CardContent className="p-0">
                        <Table>
                            <TableHeader className="bg-muted/10">
                                <TableRow>
                                    <TableHead className="font-bold text-[10px] uppercase pl-8">AÇÃO</TableHead>
                                    <TableHead className="font-bold text-[10px] uppercase">HORA DE CRIAÇÃO</TableHead>
                                    <TableHead className="font-bold text-[10px] uppercase">TEMPO PRONTO</TableHead>
                                    <TableHead className="font-bold text-[10px] uppercase text-right pr-8">STATUS</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {requests.map((request) => (
                                    <TableRow key={request.id} className="group hover:bg-muted/5 transition-colors border-none">
                                        <TableCell className="pl-8 font-semibold text-sm">
                                            {request.action}
                                        </TableCell>
                                        <TableCell className="text-sm text-muted-foreground font-medium">
                                            {request.createdAt}
                                        </TableCell>
                                        <TableCell className="text-sm text-muted-foreground font-medium">
                                            {request.readyAt}
                                        </TableCell>
                                        <TableCell className="text-right pr-8">
                                            {request.status === "PREPARANDO" ? (
                                                <span className="inline-flex items-center px-2.5 py-1 rounded-full text-[10px] font-bold bg-amber-500/10 text-amber-600 border border-amber-500/20 uppercase tracking-tight">
                                                    Preparando
                                                </span>
                                            ) : (
                                                <span className="inline-flex items-center px-2.5 py-1 rounded-full text-[10px] font-bold bg-emerald-500/10 text-emerald-600 border border-emerald-500/20 uppercase tracking-tight gap-1.5">
                                                    <CheckCircle2 size={10} />
                                                    Concluído
                                                </span>
                                            )}
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                        <div className="p-4 bg-muted/5 border-t px-8">
                            <span className="text-xs font-bold text-muted-foreground/60">{requests.length} total</span>
                        </div>
                    </CardContent>
                </Card>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pb-12">
                    <Card className="border-none shadow-lg bg-emerald-500/5 border border-emerald-500/10">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2 text-emerald-700">
                                <ShieldCheck size={20} />
                                Exportação de Dados
                            </CardTitle>
                        </CardHeader>
                        <CardContent>
                            <p className="text-sm text-emerald-700/80 leading-relaxed">
                                Você pode solicitar uma cópia de todos os seus dados pessoais em um formato estruturado (JSON/CSV).
                                O processamento pode levar até 24 horas.
                            </p>
                        </CardContent>
                        <CardFooter>
                            <Button variant="outline" className="border-emerald-200 text-emerald-700 hover:bg-emerald-100" onClick={handleRequestData}>
                                Saber mais
                            </Button>
                        </CardFooter>
                    </Card>

                    <Card className="border-none shadow-lg bg-rose-500/5 border border-rose-500/10">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2 text-rose-700">
                                <Trash2 size={20} />
                                Direito ao Esquecimento
                            </CardTitle>
                        </CardHeader>
                        <CardContent>
                            <p className="text-sm text-rose-700/80 leading-relaxed">
                                De acordo com a LGPD, você tem o direito de solicitar a exclusão total de seus dados e o encerramento da sua conta.
                                Esta ação é <strong>irreversível</strong>.
                            </p>
                        </CardContent>
                        <CardFooter>
                            <Button variant="outline" className="border-rose-200 text-rose-700 hover:bg-rose-100" onClick={() => setIsDeleteModalOpen(true)}>
                                Iniciar processo
                            </Button>
                        </CardFooter>
                    </Card>
                </div>

                {/* Confirm Delete Modal */}
                <Dialog open={isDeleteModalOpen} onOpenChange={setIsDeleteModalOpen}>
                    <DialogContent className="max-w-md p-0 overflow-hidden border-none shadow-3xl text-center">
                        <div className="p-8 space-y-6">
                            <div className="mx-auto size-20 rounded-full bg-amber-500/10 flex items-center justify-center text-amber-500 animate-bounce duration-1000">
                                <AlertTriangle size={40} />
                            </div>

                            <div className="space-y-2">
                                <DialogTitle className="text-xl font-bold">Aviso de Segurança</DialogTitle>
                                <DialogDescription className="text-base text-foreground font-medium px-4 leading-relaxed">
                                    Excluir esses dados removerá sua conta e você não fará mais login no aplicativo! Tem certeza de que deseja prosseguir?
                                </DialogDescription>
                            </div>

                            <div className="flex items-center gap-3 justify-center pt-2">
                                <Button
                                    variant="outline"
                                    className="px-8 h-11 font-bold rounded-xl"
                                    onClick={() => setIsDeleteModalOpen(false)}
                                >
                                    Cancelar
                                </Button>
                                <Button
                                    className="px-10 h-11 font-bold bg-blue-600 hover:bg-blue-700 shadow-xl shadow-blue-500/20 rounded-xl"
                                    onClick={handleDeleteAccount}
                                >
                                    Sim
                                </Button>
                            </div>
                        </div>
                    </DialogContent>
                </Dialog>
            </div>
        </AppShell>
    );
}
