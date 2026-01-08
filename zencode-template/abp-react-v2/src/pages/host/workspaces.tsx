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
import { Building2, Search, Users, Plus, Settings, ExternalLink, Eye, EyeOff, Check, Info, Activity } from "lucide-react";
import { useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter
} from "@/components/ui/dialog";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue
} from "@/components/ui/select";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

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
    const [isCreateOpen, setIsCreateOpen] = useState(false);
    const [isEditOpen, setIsEditOpen] = useState(false);
    const [selectedWorkspace, setSelectedWorkspace] = useState<any>(null);
    const [showPassword, setShowPassword] = useState(false);
    const [isSharedDb, setIsSharedDb] = useState(false);
    const [activeTab, setActiveTab] = useState("base");

    const [form, setForm] = useState({
        name: "",
        slug: "",
        edition: "none",
        adminEmail: "admin",
        password: "",
        isActive: "active",
        connectionString: ""
    });

    const [slugStatus, setSlugStatus] = useState<"idle" | "checking" | "available" | "taken">("idle");
    const [slugSuggestion, setSlugSuggestion] = useState("");

    const generateSlug = (name: string) => {
        return name
            .toLowerCase()
            .normalize("NFD")
            .replace(/[\u0300-\u036f]/g, "")
            .replace(/[^a-z0-9]+/g, "-")
            .replace(/(^-|-$)+/g, "");
    };

    const checkAvailability = (slug: string) => {
        if (!slug) {
            setSlugStatus("idle");
            return;
        }
        setSlugStatus("checking");
        setTimeout(() => {
            if (["sapienza", "educatec", "demo"].includes(slug)) {
                setSlugStatus("taken");
                setSlugSuggestion(`${slug}${Math.floor(Math.random() * 1000)}`);
            } else {
                setSlugStatus("available");
            }
        }, 600);
    };

    const handleNameChange = (name: string) => {
        const slug = generateSlug(name);
        setForm(prev => ({ ...prev, name, slug }));
        checkAvailability(slug);
    };

    const handleSlugChange = (slug: string) => {
        const formatted = generateSlug(slug);
        setForm(prev => ({ ...prev, slug: formatted }));
        checkAvailability(formatted);
    };

    const applySuggestion = () => {
        setForm(prev => ({ ...prev, slug: slugSuggestion }));
        setSlugStatus("available");
        setSlugSuggestion("");
    };

    const handleOpenCreate = () => {
        setForm({
            name: "",
            slug: "",
            edition: "none",
            adminEmail: "admin",
            password: "",
            isActive: "active",
            connectionString: ""
        });
        setSlugStatus("idle");
        setIsSharedDb(true);
        setActiveTab("base");
        setIsCreateOpen(true);
    };

    const handleOpenEdit = (workspace: any) => {
        setSelectedWorkspace(workspace);
        setForm({
            name: workspace.name,
            slug: workspace.slug,
            edition: workspace.plan.toLowerCase(),
            adminEmail: workspace.ownerEmail,
            password: "", // Not used in edit usually
            isActive: workspace.status.toLowerCase(),
            connectionString: "" // Mocked
        });
        setSlugStatus("available");
        setIsSharedDb(false); // Mocked
        setActiveTab("base");
        setIsEditOpen(true);
    };

    const handleOpenExternal = (slug: string) => {
        window.open(`/${slug}`, "_blank");
    };

    const handleSave = () => {
        toast.success(isEditOpen ? "Ambiente atualizado com sucesso!" : "Ambiente criado com sucesso!");
        setIsCreateOpen(false);
        setIsEditOpen(false);
    };

    const handleTestConnection = () => {
        toast.info("Testando conexão...");
        setTimeout(() => toast.success("Conexão estabelecida com sucesso!"), 1000);
    };

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
                                <Button className="gap-2 shadow-lg shadow-primary/20" onClick={handleOpenCreate}>
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
                                                <Button variant="ghost" size="icon" onClick={() => handleOpenEdit(workspace)}>
                                                    <Settings className="h-4 w-4" />
                                                </Button>
                                                <Button variant="ghost" size="icon" onClick={() => handleOpenExternal(workspace.slug)}>
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

            {/* New Workspace Modal */}
            <Dialog open={isCreateOpen} onOpenChange={setIsCreateOpen}>
                <DialogContent className="max-w-2xl p-0 overflow-hidden border-none shadow-2xl">
                    <DialogHeader className="p-6 bg-background border-b rounded-t-lg">
                        <DialogTitle className="text-xl font-bold">Novo ambiente</DialogTitle>
                    </DialogHeader>

                    <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                        <div className="px-6 border-b bg-background">
                            <TabsList className="h-14 bg-transparent p-0 gap-8">
                                <TabsTrigger value="base" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-pink-500 data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:text-pink-600 px-0 text-sm font-bold transition-all uppercase tracking-wide">
                                    Informação de base
                                </TabsTrigger>
                                <TabsTrigger value="connection" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-pink-500 data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:text-pink-600 px-0 text-sm font-bold transition-all uppercase tracking-wide">
                                    Strings de conexão
                                </TabsTrigger>
                            </TabsList>
                        </div>

                        <div className="p-8 pb-10 min-h-[400px]">
                            <TabsContent value="base" className="m-0 space-y-6 animate-in fade-in slide-in-from-left-2 duration-300">
                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Nome do ambiente *</Label>
                                    <Input
                                        value={form.name}
                                        onChange={e => handleNameChange(e.target.value)}
                                        className="h-12 bg-slate-50 border-slate-200 focus:bg-background transition-all"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Slug do ambiente *</Label>
                                    <div className="relative">
                                        <Input
                                            value={form.slug}
                                            onChange={e => handleSlugChange(e.target.value)}
                                            className={cn(
                                                "h-12 bg-slate-50 border-slate-200 focus:bg-background transition-all pr-32 font-medium",
                                                slugStatus === "available" && "border-emerald-500/50 bg-emerald-50/10",
                                                slugStatus === "taken" && "border-rose-500/50 bg-rose-50/10"
                                            )}
                                        />
                                        <div className="absolute right-3 top-1/2 -translate-y-1/2 flex items-center gap-2">
                                            {slugStatus === "checking" && <Activity className="size-4 animate-spin text-primary" />}
                                            {slugStatus === "available" && <Badge className="bg-emerald-500 hover:bg-emerald-600 text-[10px] font-bold">DISPONÍVEL</Badge>}
                                            {slugStatus === "taken" && <Badge className="bg-rose-500 hover:bg-rose-600 text-[10px] font-bold">INDISPONÍVEL</Badge>}
                                        </div>
                                    </div>
                                    {slugStatus === "taken" && slugSuggestion && (
                                        <p className="text-[11px] font-bold text-slate-500 animate-in fade-in slide-in-from-top-1">
                                            Sugestão: <button onClick={applySuggestion} className="text-primary hover:underline">{slugSuggestion}</button>
                                        </p>
                                    )}
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700 font-bold">Edição</Label>
                                    <Select value={form.edition} onValueChange={v => setForm({ ...form, edition: v })}>
                                        <SelectTrigger className="h-12 bg-slate-50 border-slate-200">
                                            <SelectValue placeholder="Selecionar edição" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="none">Não atribuído</SelectItem>
                                            <SelectItem value="starter">Starter</SelectItem>
                                            <SelectItem value="professional">Professional</SelectItem>
                                            <SelectItem value="enterprise">Enterprise</SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Endereço de e-mail do administrador do ambiente *</Label>
                                    <Input
                                        value={form.adminEmail}
                                        onChange={e => setForm({ ...form, adminEmail: e.target.value })}
                                        className="h-12 bg-slate-50 border-slate-200 text-blue-800 font-medium"
                                    />
                                    <p className="text-[11px] text-muted-foreground font-medium flex items-center gap-1">
                                        <Info size={12} />
                                        O e-mail do locatário não pode ser alterado após a criação
                                    </p>
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Senha do administrador do ambiente *</Label>
                                    <div className="relative">
                                        <Input
                                            type={showPassword ? "text" : "password"}
                                            value={form.password}
                                            onChange={e => setForm({ ...form, password: e.target.value })}
                                            className="h-12 bg-slate-50 border-slate-200 pr-12 focus:bg-background transition-all"
                                            placeholder="••••••"
                                        />
                                        <Button
                                            variant="ghost"
                                            size="icon"
                                            className="absolute right-0 top-0 h-12 w-12 rounded-r-lg bg-indigo-500/10 hover:bg-indigo-500/20 text-indigo-600 border-l border-slate-200/50"
                                            onClick={() => setShowPassword(!showPassword)}
                                        >
                                            {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                                        </Button>
                                    </div>
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Estado de ativação do ambiente *</Label>
                                    <Select value={form.isActive} onValueChange={v => setForm({ ...form, isActive: v })}>
                                        <SelectTrigger className="h-12 bg-slate-50 border-slate-200">
                                            <SelectValue />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="active">Ativo</SelectItem>
                                            <SelectItem value="inactive">Inativo</SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>
                            </TabsContent>

                            <TabsContent value="connection" className="m-0 space-y-8 animate-in fade-in slide-in-from-right-2 duration-300">
                                <div className="flex items-center space-x-3 p-4 rounded-xl bg-slate-50 border-2 border-dashed border-slate-200 hover:bg-slate-100/50 transition-all cursor-pointer group" onClick={() => setIsSharedDb(!isSharedDb)}>
                                    <Checkbox checked={isSharedDb} onCheckedChange={(v) => setIsSharedDb(v as boolean)} className="size-5 rounded border-2" />
                                    <Label className="text-sm font-bold text-slate-600 cursor-pointer group-hover:text-slate-900 transition-colors">Usar o banco de dados compartilhado</Label>
                                </div>

                                {!isSharedDb && (
                                    <div className="space-y-3 animate-in fade-in zoom-in-95 duration-200">
                                        <Label className="text-sm font-bold text-slate-700">String de conexão padrão*</Label>
                                        <div className="flex gap-3">
                                            <Input
                                                value={form.connectionString}
                                                onChange={e => setForm({ ...form, connectionString: e.target.value })}
                                                placeholder="Server=localhost;Database=..."
                                                className="h-12 bg-slate-50 border-slate-200 font-mono text-sm flex-1"
                                            />
                                            <Button
                                                variant="outline"
                                                onClick={handleTestConnection}
                                                className="h-12 px-6 bg-[#3b66f5] hover:bg-[#2d52d9] text-white border-none font-bold shadow-lg shadow-blue-500/20 transition-all"
                                            >
                                                Teste
                                            </Button>
                                        </div>
                                    </div>
                                )}
                            </TabsContent>
                        </div>
                    </Tabs>

                    <DialogFooter className="p-6 bg-slate-50 border-t gap-3 rounded-b-lg">
                        <Button variant="outline" onClick={() => setIsCreateOpen(false)} className="px-8 h-12 font-bold border-2 border-slate-200 text-slate-600 hover:bg-white transition-all rounded-xl">
                            Cancelar
                        </Button>
                        <Button onClick={handleSave} className="px-10 h-12 font-bold bg-[#3b66f5] hover:bg-[#2d52d9] text-white shadow-lg shadow-blue-500/20 transition-all rounded-xl gap-2">
                            <Check size={18} />
                            Salvar
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>

            {/* Edit Workspace Modal */}
            <Dialog open={isEditOpen} onOpenChange={setIsEditOpen}>
                <DialogContent className="max-w-2xl p-0 overflow-hidden border-none shadow-2xl">
                    <DialogHeader className="p-6 bg-background border-b rounded-t-lg">
                        <DialogTitle className="text-xl font-bold">Editar ambiente - <span className="text-primary">{selectedWorkspace?.name}</span></DialogTitle>
                    </DialogHeader>

                    <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                        <div className="px-6 border-b bg-background">
                            <TabsList className="h-14 bg-transparent p-0 gap-8">
                                <TabsTrigger value="base" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-pink-500 data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:text-pink-600 px-0 text-sm font-bold transition-all uppercase tracking-wide">
                                    Informação de base
                                </TabsTrigger>
                                <TabsTrigger value="connection" className="h-full rounded-none border-b-2 border-transparent data-[state=active]:border-pink-500 data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:text-pink-600 px-0 text-sm font-bold transition-all uppercase tracking-wide">
                                    Strings de conexão
                                </TabsTrigger>
                            </TabsList>
                        </div>

                        <div className="p-8 pb-10 min-h-[400px]">
                            <TabsContent value="base" className="m-0 space-y-6 animate-in fade-in slide-in-from-left-2 duration-300">
                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Nome do ambiente *</Label>
                                    <Input
                                        value={form.name}
                                        onChange={e => handleNameChange(e.target.value)}
                                        className="h-12 bg-slate-50 border-slate-200 focus:bg-background transition-all"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Slug do ambiente *</Label>
                                    <div className="relative">
                                        <Input
                                            value={form.slug}
                                            onChange={e => handleSlugChange(e.target.value)}
                                            className={cn(
                                                "h-12 bg-slate-50 border-slate-200 focus:bg-background transition-all pr-32 font-medium",
                                                slugStatus === "available" && "border-emerald-500/50 bg-emerald-50/10",
                                                slugStatus === "taken" && "border-rose-500/50 bg-rose-50/10"
                                            )}
                                        />
                                        <div className="absolute right-3 top-1/2 -translate-y-1/2 flex items-center gap-2">
                                            {slugStatus === "checking" && <Activity className="size-4 animate-spin text-primary" />}
                                            {slugStatus === "available" && <Badge className="bg-emerald-500 hover:bg-emerald-600 text-[10px] font-bold">DISPONÍVEL</Badge>}
                                            {slugStatus === "taken" && <Badge className="bg-rose-500 hover:bg-rose-600 text-[10px] font-bold">INDISPONÍVEL</Badge>}
                                        </div>
                                    </div>
                                    {slugStatus === "taken" && slugSuggestion && (
                                        <p className="text-[11px] font-bold text-slate-500 animate-in fade-in slide-in-from-top-1">
                                            Sugestão: <button onClick={applySuggestion} className="text-primary hover:underline">{slugSuggestion}</button>
                                        </p>
                                    )}
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700 font-bold">Edição</Label>
                                    <Select value={form.edition} onValueChange={v => setForm({ ...form, edition: v })}>
                                        <SelectTrigger className="h-12 bg-slate-50 border-slate-200">
                                            <SelectValue placeholder="Selecionar edição" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="none">Não atribuído</SelectItem>
                                            <SelectItem value="starter">Starter</SelectItem>
                                            <SelectItem value="professional">Professional</SelectItem>
                                            <SelectItem value="enterprise">Enterprise</SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>

                                <div className="space-y-2">
                                    <Label className="text-sm font-bold text-slate-700">Estado de ativação do ambiente *</Label>
                                    <Select value={form.isActive} onValueChange={v => setForm({ ...form, isActive: v })}>
                                        <SelectTrigger className="h-12 bg-slate-50 border-slate-200">
                                            <SelectValue />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="active">Ativo</SelectItem>
                                            <SelectItem value="inactive">Inativo</SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>
                            </TabsContent>

                            <TabsContent value="connection" className="m-0 space-y-8 animate-in fade-in slide-in-from-right-2 duration-300">
                                <div className="flex items-center space-x-3 p-4 rounded-xl bg-slate-50 border-2 border-dashed border-slate-200 hover:bg-slate-100/50 transition-all cursor-pointer group" onClick={() => setIsSharedDb(!isSharedDb)}>
                                    <Checkbox checked={isSharedDb} onCheckedChange={(v) => setIsSharedDb(v as boolean)} className="size-5 rounded border-2" />
                                    <Label className="text-sm font-bold text-slate-600 cursor-pointer group-hover:text-slate-900 transition-colors">Usar o banco de dados compartilhado</Label>
                                </div>

                                {!isSharedDb && (
                                    <div className="space-y-3 animate-in fade-in zoom-in-95 duration-200">
                                        <Label className="text-sm font-bold text-slate-700">String de conexão padrão*</Label>
                                        <div className="flex gap-3">
                                            <Input
                                                value={form.connectionString}
                                                onChange={e => setForm({ ...form, connectionString: e.target.value })}
                                                placeholder="Server=localhost;Database=..."
                                                className="h-12 bg-slate-50 border-slate-200 font-mono text-sm flex-1"
                                            />
                                            <Button
                                                variant="outline"
                                                onClick={handleTestConnection}
                                                className="h-12 px-6 bg-[#3b66f5] hover:bg-[#2d52d9] text-white border-none font-bold shadow-lg shadow-blue-500/20 transition-all"
                                            >
                                                Teste
                                            </Button>
                                        </div>
                                    </div>
                                )}
                            </TabsContent>
                        </div>
                    </Tabs>

                    <DialogFooter className="p-6 bg-slate-50 border-t gap-3 rounded-b-lg">
                        <Button variant="outline" onClick={() => setIsEditOpen(false)} className="px-8 h-12 font-bold border-2 border-slate-200 text-slate-600 hover:bg-white transition-all rounded-xl">
                            Cancelar
                        </Button>
                        <Button onClick={handleSave} className="px-10 h-12 font-bold bg-[#3b66f5] hover:bg-[#2d52d9] text-white shadow-lg shadow-blue-500/20 transition-all rounded-xl gap-2">
                            <Check size={18} />
                            Salvar
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </AppShell>
    );
}
