import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Checkbox } from "@/components/ui/checkbox";
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
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
} from "@/components/ui/dialog";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
    DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import {
    Users,
    Search,
    Plus,
    Mail,
    Shield,
    MoreHorizontal,
    Eye,
    Edit,
    Lock,
    History,
    Trash2,
    CheckCircle2,
    XCircle,
    UserCircle,
    Building2,
    Settings2,
    Fingerprint,
    Smartphone,
    Info,
    ShieldCheck,
    Save,
    Camera,
    Clock
} from "lucide-react";
import { useState } from "react";
import { cn } from "@/lib/utils";
import { toast } from "sonner";

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
    const [selectedUser, setSelectedUser] = useState<any>(null);
    const [isDetailsOpen, setIsDetailsOpen] = useState(false);
    const [isEditOpen, setIsEditOpen] = useState(false);
    const [isUpdating, setIsUpdating] = useState(false);

    // Form states
    const [editForm, setEditForm] = useState({
        userName: "",
        name: "",
        surname: "",
        email: "",
        phoneNumber: "",
        isActive: true,
    });

    const handleOpenDetails = (user: any) => {
        setSelectedUser(user);
        setIsDetailsOpen(true);
    };

    const handleOpenEdit = (user: any) => {
        setSelectedUser(user);
        setEditForm({
            userName: user.userName,
            name: user.name.split(' ')[0],
            surname: user.name.split(' ').slice(1).join(' '),
            email: user.email,
            phoneNumber: user.phoneNumber || "",
            isActive: user.isActive,
        });
        setIsEditOpen(true);
    };

    const handleSaveUser = async (e: React.FormEvent) => {
        if (e) e.preventDefault();
        setIsUpdating(true);
        // Simulate API call
        setTimeout(() => {
            toast.success("Usuário atualizado com sucesso!");
            setIsUpdating(false);
            setIsEditOpen(false);
        }, 1000);
    };

    return (
        <AppShell>
            <div className="space-y-6">
                {/* Page Header */}
                <div className="flex items-center gap-3">
                    <Users className="h-8 w-8 text-primary" />
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Usuários</h1>
                        <p className="text-muted-foreground">
                            Gerencie os usuários de todos os espaços de trabalho.
                        </p>
                    </div>
                </div>

                {/* Users Table */}
                <Card className="border-none shadow-sm overflow-hidden">
                    <CardHeader className="pb-4">
                        <div className="flex items-center justify-between">
                            <CardTitle className="text-xl">Usuários ({mockUsers.length})</CardTitle>
                            <div className="flex items-center gap-4">
                                <div className="relative w-80">
                                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                    <Input
                                        placeholder="Pesquisar por nome, email ou usuário..."
                                        className="pl-10"
                                    />
                                </div>
                                <Button className="gap-2 shadow-lg shadow-primary/20">
                                    <Plus className="size-4" />
                                    Novo Usuário
                                </Button>
                            </div>
                        </div>
                    </CardHeader>
                    <CardContent className="p-0">
                        <Table>
                            <TableHeader className="bg-muted/30">
                                <TableRow>
                                    <TableHead className="w-[100px]">Ações</TableHead>
                                    <TableHead>Usuário</TableHead>
                                    <TableHead>Email</TableHead>
                                    <TableHead>Telefone</TableHead>
                                    <TableHead>Funções</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead>Criado em</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {mockUsers.map((user) => (
                                    <TableRow key={user.id} className="group transition-colors hover:bg-muted/20">
                                        <TableCell>
                                            <DropdownMenu>
                                                <DropdownMenuTrigger asChild>
                                                    <Button variant="secondary" size="sm" className="h-8 gap-2 bg-primary/10 text-primary hover:bg-primary hover:text-white transition-all">
                                                        <Settings2 className="size-3.5" />
                                                        Ações
                                                        <MoreHorizontal className="size-3.5" />
                                                    </Button>
                                                </DropdownMenuTrigger>
                                                <DropdownMenuContent align="start" className="w-56">
                                                    <DropdownMenuItem className="gap-2 cursor-pointer" onClick={() => handleOpenDetails(user)}>
                                                        <Eye className="size-4" />
                                                        Ver detalhes
                                                    </DropdownMenuItem>
                                                    <DropdownMenuItem className="gap-2 cursor-pointer" onClick={() => handleOpenEdit(user)}>
                                                        <Edit className="size-4" />
                                                        Editar
                                                    </DropdownMenuItem>
                                                    <DropdownMenuItem className="gap-2 cursor-pointer">
                                                        <History className="size-4" />
                                                        Solicitações
                                                    </DropdownMenuItem>
                                                    <DropdownMenuItem className="gap-2 cursor-pointer">
                                                        <Shield className="size-4" />
                                                        Permissões
                                                    </DropdownMenuItem>
                                                    <DropdownMenuItem className="gap-2 cursor-pointer">
                                                        <Lock className="size-4" />
                                                        Configurar senha
                                                    </DropdownMenuItem>
                                                    <DropdownMenuItem className="gap-2 cursor-pointer">
                                                        <Fingerprint className="size-4" />
                                                        Dois fatores
                                                    </DropdownMenuItem>
                                                    <DropdownMenuSeparator />
                                                    <DropdownMenuItem className="gap-2 cursor-pointer text-destructive focus:text-destructive">
                                                        <Trash2 className="size-4" />
                                                        Excluir
                                                    </DropdownMenuItem>
                                                </DropdownMenuContent>
                                            </DropdownMenu>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-3">
                                                <div className="h-9 w-9 rounded-full bg-primary/10 flex items-center justify-center border border-primary/20">
                                                    <span className="text-sm font-bold text-primary">
                                                        {user.name.split(' ').map(n => n[0]).join('').slice(0, 2).toUpperCase()}
                                                    </span>
                                                </div>
                                                <div>
                                                    <p className="font-semibold text-sm">{user.name}</p>
                                                    <p className="text-xs text-muted-foreground font-mono">@{user.userName}</p>
                                                </div>
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-2">
                                                <Mail className="h-3.5 w-3.5 text-muted-foreground" />
                                                <span className="text-sm font-medium">{user.email}</span>
                                            </div>
                                        </TableCell>
                                        <TableCell className="text-sm">
                                            {user.phoneNumber || <span className="text-muted-foreground/50">—</span>}
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex gap-1 flex-wrap">
                                                {user.roles.map((role) => (
                                                    <Badge key={role} variant="outline" className="gap-1 bg-muted/50 text-[10px] font-bold uppercase tracking-wider">
                                                        <Shield className="h-2.5 w-2.5" />
                                                        {role}
                                                    </Badge>
                                                ))}
                                            </div>
                                        </TableCell>
                                        <TableCell>
                                            <Badge variant={user.isActive ? "secondary" : "destructive"} className={cn(
                                                "gap-1.5",
                                                user.isActive ? "bg-emerald-500/10 text-emerald-600 border-emerald-500/20" : ""
                                            )}>
                                                {user.isActive ? <CheckCircle2 className="size-3" /> : <XCircle className="size-3" />}
                                                {user.isActive ? "Ativo" : "Inativo"}
                                            </Badge>
                                        </TableCell>
                                        <TableCell className="text-sm font-mono text-muted-foreground">
                                            {new Date(user.createdAt).toLocaleDateString("pt-BR")}
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>
            </div>

            {/* User Details Modal */}
            <Dialog open={isDetailsOpen} onOpenChange={setIsDetailsOpen}>
                <DialogContent className="max-w-2xl p-0 overflow-hidden border-none shadow-2xl">
                    <DialogHeader className="p-6 bg-primary/5 border-b sticky top-0 z-10">
                        <DialogTitle className="text-2xl flex items-center gap-3">
                            <div className="size-10 rounded-xl bg-primary/10 flex items-center justify-center text-primary">
                                <UserCircle className="size-6" />
                            </div>
                            Ver detalhes
                        </DialogTitle>
                    </DialogHeader>

                    {selectedUser && (
                        <Tabs defaultValue="info" className="w-full">
                            <div className="px-6 border-b bg-background">
                                <TabsList className="h-12 bg-transparent p-0 gap-6">
                                    <TabsTrigger value="info" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold">
                                        Informações do Usuário
                                    </TabsTrigger>
                                    <TabsTrigger value="org" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold">
                                        Unidades organizacionais (0)
                                    </TabsTrigger>
                                    <TabsTrigger value="general" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold">
                                        Geral
                                    </TabsTrigger>
                                </TabsList>
                            </div>

                            <ScrollArea className="max-h-[60vh]">
                                <TabsContent value="info" className="p-6 mt-0">
                                    <div className="divide-y border rounded-xl overflow-hidden shadow-sm">
                                        {[
                                            { label: "Nome do usuário", value: selectedUser.userName, bold: true },
                                            { label: "Nome", value: selectedUser.name.split(' ')[0], bold: true },
                                            { label: "Sobrenome", value: selectedUser.name.split(' ').slice(1).join(' ') || "-", bold: false },
                                            { label: "Email", value: selectedUser.email, bold: true },
                                            { label: "Grupos de permissões", value: selectedUser.roles.join(', '), bold: true },
                                            { label: "Número de telefone", value: selectedUser.phoneNumber || "-", bold: false },
                                            { label: "Ativo", value: selectedUser.isActive ? "Sim" : "Não", bold: true, colored: true, success: selectedUser.isActive },
                                            { label: "Forçar alteração de senha", value: "Não", bold: true },
                                            { label: "Bloquear conta após tentativas de login malsucedidas", value: "Sim", bold: true },
                                            { label: "É externo", value: "Não", bold: true },
                                        ].map((row, i) => (
                                            <div key={i} className="flex px-4 py-3 bg-white hover:bg-muted/5 transition-colors">
                                                <div className="w-1/2 text-sm text-muted-foreground font-medium">{row.label}</div>
                                                <div className={cn(
                                                    "w-1/2 text-sm",
                                                    row.bold && "font-bold text-foreground",
                                                    row.colored && (row.success ? "text-emerald-600" : "text-rose-600")
                                                )}>
                                                    {row.value}
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                </TabsContent>

                                <TabsContent value="org" className="p-12 mt-0 flex flex-col items-center justify-center text-center space-y-4 opacity-70">
                                    <div className="size-16 rounded-full bg-muted flex items-center justify-center">
                                        <Building2 className="size-8 text-muted-foreground" />
                                    </div>
                                    <p className="text-sm font-medium text-muted-foreground">Sem unidades organizacionais!</p>
                                </TabsContent>

                                <TabsContent value="general" className="p-6 mt-0">
                                    <div className="divide-y border rounded-xl overflow-hidden shadow-sm">
                                        {[
                                            { label: "Criado por", value: "-" },
                                            { label: "Hora de criação", value: "07/01/2026 20:00", bold: true },
                                            { label: "Modificado por", value: "-" },
                                            { label: "Hora da modificação", value: "07/01/2026 20:00", bold: true },
                                            { label: "Hora de atualização da senha", value: "07/01/2026 20:00", bold: true },
                                            { label: "Hora de término do bloqueio", value: "-" },
                                            { label: "Contagem de acessos com falha", value: "0", bold: true },
                                        ].map((row, i) => (
                                            <div key={i} className="flex px-4 py-3 bg-white hover:bg-muted/5 transition-colors">
                                                <div className="w-1/2 text-sm text-muted-foreground font-medium">{row.label}</div>
                                                <div className={cn("w-1/2 text-sm", row.bold && "font-bold text-foreground")}>{row.value}</div>
                                            </div>
                                        ))}
                                    </div>
                                </TabsContent>
                            </ScrollArea>
                        </Tabs>
                    )}

                    <DialogFooter className="p-6 bg-muted/30 border-t shrink-0">
                        <Button variant="outline" onClick={() => setIsDetailsOpen(false)} className="w-full h-12 text-base font-bold bg-primary text-white hover:bg-primary/90 hover:text-white border-none shadow-lg shadow-primary/30 active:scale-95 transition-all">
                            Fechar
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>

            {/* Edit User Modal - MIRRORS PROFILE PAGE */}
            <Dialog open={isEditOpen} onOpenChange={setIsEditOpen}>
                <DialogContent className="max-w-4xl p-0 overflow-hidden border-none shadow-2xl">
                    <DialogHeader className="p-6 bg-primary text-primary-foreground">
                        <DialogTitle className="text-2xl flex items-center gap-3 font-bold">
                            <Edit className="size-6" />
                            Editar Usuário: {selectedUser?.userName}
                        </DialogTitle>
                    </DialogHeader>

                    <div className="grid grid-cols-1 md:grid-cols-12 overflow-hidden">
                        {/* Sidebar Summary */}
                        <div className="md:col-span-4 bg-muted/30 p-8 border-r space-y-6 hidden md:block">
                            <div className="text-center space-y-4">
                                <div className="relative group mx-auto mb-4 w-32">
                                    <Avatar className="size-32 border-4 border-background shadow-xl mx-auto ring-4 ring-primary/5 transition-transform group-hover:scale-105 duration-300">
                                        <AvatarImage src={`https://api.dicebear.com/7.x/avataaars/svg?seed=${selectedUser?.userName}`} />
                                        <AvatarFallback className="text-3xl bg-primary/10 text-primary font-bold">
                                            {selectedUser?.name?.[0]}{selectedUser?.userName?.[0]}
                                        </AvatarFallback>
                                    </Avatar>
                                    <button className="absolute bottom-1 right-2 p-2 bg-primary text-primary-foreground rounded-full shadow-lg hover:scale-110 transition-transform">
                                        <Camera size={16} />
                                    </button>
                                </div>
                                <div>
                                    <h3 className="text-xl font-bold">{selectedUser?.name}</h3>
                                    <p className="text-sm text-muted-foreground font-mono">@{selectedUser?.userName}</p>
                                </div>
                                <Badge variant="secondary" className="bg-emerald-500/10 text-emerald-600 border-emerald-500/20 gap-1.5 py-1 px-3 rounded-full">
                                    <CheckCircle2 size={12} />
                                    Conta Ativa
                                </Badge>
                            </div>

                            <div className="space-y-4 pt-6 border-t">
                                <div className="space-y-1">
                                    <p className="text-[10px] font-bold text-muted-foreground uppercase tracking-widest">E-mail</p>
                                    <p className="text-sm font-medium truncate">{selectedUser?.email}</p>
                                </div>
                                <div className="space-y-1">
                                    <p className="text-[10px] font-bold text-muted-foreground uppercase tracking-widest">Id do Usuário</p>
                                    <p className="text-xs font-mono truncate">{selectedUser?.id}</p>
                                </div>
                            </div>
                        </div>

                        {/* Tabs Content */}
                        <div className="md:col-span-8 flex flex-col">
                            <Tabs defaultValue="info" className="flex-1 flex flex-col overflow-hidden">
                                <div className="px-6 border-b bg-background sticky top-0 z-10">
                                    <TabsList className="h-14 bg-transparent p-0 gap-6 overflow-x-auto no-scrollbar">
                                        <TabsTrigger value="info" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                            <UserCircle size={16} />
                                            Informações
                                        </TabsTrigger>
                                        <TabsTrigger value="roles" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                            <Shield size={16} />
                                            Funções
                                        </TabsTrigger>
                                        <TabsTrigger value="org" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-primary data-[state=active]:bg-transparent data-[state=active]:shadow-none px-0 text-sm font-bold gap-2 shrink-0">
                                            <Building2 size={16} />
                                            Unidades
                                        </TabsTrigger>
                                    </TabsList>
                                </div>

                                <ScrollArea className="flex-1 scroll-smooth">
                                    <div className="p-8">
                                        {/* TAB: Informações */}
                                        <TabsContent value="info" className="p-0 m-0 space-y-6">
                                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                                                <div className="space-y-2 sm:col-span-2">
                                                    <Label htmlFor="edit-username" className="text-xs font-bold uppercase text-muted-foreground">Nome do usuário *</Label>
                                                    <Input
                                                        id="edit-username"
                                                        value={editForm.userName}
                                                        onChange={e => setEditForm({ ...editForm, userName: e.target.value })}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all font-medium"
                                                    />
                                                </div>
                                                <div className="space-y-2">
                                                    <Label htmlFor="edit-name" className="text-xs font-bold uppercase text-muted-foreground">Nome</Label>
                                                    <Input
                                                        id="edit-name"
                                                        value={editForm.name}
                                                        onChange={e => setEditForm({ ...editForm, name: e.target.value })}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all font-medium"
                                                    />
                                                </div>
                                                <div className="space-y-2">
                                                    <Label htmlFor="edit-surname" className="text-xs font-bold uppercase text-muted-foreground">Sobrenome</Label>
                                                    <Input
                                                        id="edit-surname"
                                                        value={editForm.surname}
                                                        onChange={e => setEditForm({ ...editForm, surname: e.target.value })}
                                                        className="h-11 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all font-medium"
                                                    />
                                                </div>
                                                <div className="space-y-2 sm:col-span-2">
                                                    <Label htmlFor="edit-email" className="text-xs font-bold uppercase text-muted-foreground">Endereço de e-mail *</Label>
                                                    <div className="relative">
                                                        <Mail className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
                                                        <Input
                                                            id="edit-email"
                                                            type="email"
                                                            value={editForm.email}
                                                            onChange={e => setEditForm({ ...editForm, email: e.target.value })}
                                                            className="h-11 pl-10 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all font-medium"
                                                        />
                                                    </div>
                                                </div>
                                                <div className="space-y-2 sm:col-span-2">
                                                    <Label htmlFor="edit-phone" className="text-xs font-bold uppercase text-muted-foreground">Número de telefone</Label>
                                                    <div className="relative">
                                                        <Smartphone className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
                                                        <Input
                                                            id="edit-phone"
                                                            value={editForm.phoneNumber}
                                                            onChange={e => setEditForm({ ...editForm, phoneNumber: e.target.value })}
                                                            className="h-11 pl-10 bg-muted/20 border-muted-foreground/20 focus:bg-background transition-all font-medium"
                                                            placeholder="(00) 00000-0000"
                                                        />
                                                    </div>
                                                </div>
                                            </div>

                                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 pt-4 border-t border-dashed">
                                                <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-muted/10 transition-colors cursor-pointer" onClick={() => setEditForm({ ...editForm, isActive: !editForm.isActive })}>
                                                    <Checkbox id="edit-active" checked={editForm.isActive} />
                                                    <label htmlFor="edit-active" className="text-sm font-semibold cursor-pointer">Ativo</label>
                                                </div>
                                                <div className="flex items-center gap-3 p-3 rounded-lg hover:bg-muted/10 transition-colors cursor-pointer">
                                                    <Checkbox id="edit-lockout" defaultChecked />
                                                    <div className="flex items-center gap-1.5">
                                                        <label htmlFor="edit-lockout" className="text-sm font-semibold cursor-pointer">Bloquear conta após falhas</label>
                                                        <Info size={14} className="text-muted-foreground" />
                                                    </div>
                                                </div>
                                            </div>
                                        </TabsContent>

                                        {/* TAB: Funções (Roles) */}
                                        <TabsContent value="roles" className="p-0 m-0 space-y-6">
                                            <div className="flex items-center gap-3 text-primary mb-2">
                                                <ShieldCheck size={20} className="fill-primary/10" />
                                                <h3 className="text-lg font-bold">Grupos de permissões</h3>
                                            </div>
                                            <div className="space-y-3">
                                                {["admin", "user", "manager"].map((role) => (
                                                    <div key={role} className="flex items-center justify-between p-4 rounded-xl border bg-muted/5 group hover:bg-primary/5 hover:border-primary/20 transition-all cursor-pointer" onClick={() => { }}>
                                                        <div className="flex items-center gap-4">
                                                            <div className="size-10 rounded-lg bg-background border flex items-center justify-center text-primary shadow-sm group-hover:scale-110 transition-transform">
                                                                <Shield size={20} />
                                                            </div>
                                                            <div>
                                                                <p className="font-bold text-sm uppercase tracking-wide">{role}</p>
                                                                <p className="text-xs text-muted-foreground">Grupo de permissões {role === 'admin' ? 'total' : 'limitado'}</p>
                                                            </div>
                                                        </div>
                                                        <Checkbox id={`edit-role-${role}`} checked={selectedUser?.roles.includes(role)} />
                                                    </div>
                                                ))}
                                            </div>
                                        </TabsContent>

                                        {/* TAB: Org Units */}
                                        <TabsContent value="org" className="py-12 px-4 m-0 flex flex-col items-center justify-center text-center space-y-4 opacity-70">
                                            <div className="size-20 rounded-full bg-muted flex items-center justify-center border-4 border-background shadow-inner">
                                                <Building2 className="size-10 text-muted-foreground" />
                                            </div>
                                            <p className="text-lg font-bold text-muted-foreground">Sem unidades organizacionais!</p>
                                            <p className="text-sm text-muted-foreground max-w-xs">Este usuário não está vinculado a nenhuma unidade organizacional.</p>
                                        </TabsContent>
                                    </div>
                                </ScrollArea>
                            </Tabs>
                        </div>
                    </div>

                    <DialogFooter className="p-6 bg-muted/30 border-t justify-end gap-3 shrink-0">
                        <Button variant="ghost" type="button" onClick={() => setIsEditOpen(false)} className="px-6 font-bold text-muted-foreground h-12">
                            Cancelar
                        </Button>
                        <Button type="button" onClick={handleSaveUser} disabled={isUpdating} className="px-10 font-bold shadow-lg shadow-primary/20 h-12 transition-all active:scale-95">
                            {isUpdating ? <Clock className="animate-spin size-4 mr-2" /> : <Save size={18} className="mr-2" />}
                            Salvar Alterações
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </AppShell>
    );
}
