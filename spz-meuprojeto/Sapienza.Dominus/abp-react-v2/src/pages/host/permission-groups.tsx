import { useState } from "react";
import { AppShell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Checkbox } from "@/components/ui/checkbox";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Badge } from "@/components/ui/badge";
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
import {
    Search,
    Plus,
    Shield,
    CheckCircle2,
    X,
    ChevronRight,
    SearchCode,
    Lock,
    Eye,
} from "lucide-react";
import { cn } from "@/lib/utils";

// Mock Data
const permissionGroups = [
    { id: "1", name: "admin", isPublic: true },
    { id: "2", name: "manager", isPublic: true },
    { id: "3", name: "external_user", isPublic: false },
];

const permissionCategories = [
    { id: "lepton", name: "Gerenciamento de tema Lepton", count: 1 },
    { id: "text-models", name: "Gerenciamento de modelos de texto", count: 2 },
    { id: "chat", name: "Chat", count: 1 },
    { id: "pastas", name: "Pastas", count: 8 },
    { id: "formularios", name: "Formulários", count: 4 },
    { id: "cms-public", name: "CmsKit Público", count: 2 },
    { id: "cms-admin", name: "CmsKit Admin", count: 28 },
    { id: "sapienza", name: "Sapienza.Cursos", count: 9 },
];

const permissionsByCategory: Record<string, any[]> = {
    "pastas": [
        { id: "p1", name: "Diretório", children: ["Criar", "Atualizar", "Deletar"] },
        { id: "p2", name: "Arquivo", children: ["Criar", "Atualizar", "Deletar"] },
    ],
    "lepton": [{ id: "l1", name: "Settings", children: ["View"] }],
};

export default function PermissionGroupsPage() {
    const [selectedGroup, setSelectedGroup] = useState<any>(null);
    const [isPermissionsOpen, setIsPermissionsOpen] = useState(false);
    const [activeCategory, setActiveCategory] = useState("pastas");
    const [searchQuery, setSearchQuery] = useState("");

    const handleOpenPermissions = (group: any) => {
        setSelectedGroup(group);
        setIsPermissionsOpen(true);
    };

    return (
        <AppShell>
            <div className="space-y-6">
                <div className="flex items-center justify-between">
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">Grupos de permissões</h1>
                        <p className="text-muted-foreground">Gerencie conjuntos de permissões reutilizáveis para usuários e cargos.</p>
                    </div>
                    <Button className="gap-2 shadow-lg shadow-primary/20">
                        <Plus className="size-4" />
                        Novo Grupo
                    </Button>
                </div>

                <Card className="border-none shadow-sm">
                    <CardHeader className="pb-4">
                        <div className="relative max-w-sm">
                            <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
                            <Input placeholder="Pesquisar..." className="pl-10" />
                        </div>
                    </CardHeader>
                    <CardContent className="p-0">
                        <Table>
                            <TableHeader className="bg-muted/30">
                                <TableRow>
                                    <TableHead className="w-[150px]">Ações</TableHead>
                                    <TableHead>Nome do grupo de permissões</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {permissionGroups.map((group) => (
                                    <TableRow key={group.id} className="group">
                                        <TableCell>
                                            <Button
                                                variant="outline"
                                                size="sm"
                                                className="h-8 gap-2 border-primary/20 hover:bg-primary/5 hover:border-primary/50 text-primary transition-all shadow-sm"
                                                onClick={() => handleOpenPermissions(group)}
                                            >
                                                <Shield className="size-3.5" />
                                                Gerenciar
                                            </Button>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex items-center gap-3 font-medium">
                                                {group.name}
                                                {group.isPublic && (
                                                    <Badge variant="secondary" className="bg-blue-500/10 text-blue-600 border-blue-500/20 text-[10px] font-bold uppercase tracking-wider">
                                                        Público
                                                    </Badge>
                                                )}
                                            </div>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>
            </div>

            {/* Permission Management Modal - REDESIGNED */}
            <Dialog open={isPermissionsOpen} onOpenChange={setIsPermissionsOpen}>
                <DialogContent className="max-w-5xl h-[85vh] flex flex-col p-0 overflow-hidden border-none shadow-2xl">
                    <DialogHeader className="p-6 bg-primary/5 border-b shrink-0">
                        <div className="flex items-center justify-between pr-8">
                            <div>
                                <DialogTitle className="text-2xl flex items-center gap-3">
                                    <div className="size-10 rounded-xl bg-primary/10 flex items-center justify-center text-primary">
                                        <Shield className="size-6" />
                                    </div>
                                    Permissões - <span className="text-primary">{selectedGroup?.name}</span>
                                </DialogTitle>
                                <DialogDescription className="mt-1">
                                    Defina o acesso granular para este grupo de permissões.
                                </DialogDescription>
                            </div>
                            <div className="flex items-center gap-4">
                                <div className="flex items-center gap-2 bg-background border px-3 py-1.5 rounded-lg shadow-sm">
                                    <Checkbox id="grant-all" />
                                    <label htmlFor="grant-all" className="text-sm font-semibold cursor-pointer whitespace-nowrap">
                                        Conceder todas
                                    </label>
                                </div>
                            </div>
                        </div>
                    </DialogHeader>

                    <div className="flex-1 min-h-0 flex overflow-hidden">
                        {/* Sidebar: Categories */}
                        <div className="w-[300px] border-r bg-muted/20 flex flex-col shrink-0">
                            <div className="p-4 border-b bg-background/50">
                                <div className="relative">
                                    <SearchCode className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
                                    <Input
                                        placeholder="Filtrar grupos..."
                                        className="h-9 pl-9 bg-background"
                                        value={searchQuery}
                                        onChange={(e) => setSearchQuery(e.target.value)}
                                    />
                                </div>
                            </div>
                            <ScrollArea className="flex-1">
                                <div className="p-3 space-y-1">
                                    {permissionCategories.map((cat) => (
                                        <button
                                            key={cat.id}
                                            onClick={() => setActiveCategory(cat.id)}
                                            className={cn(
                                                "w-full flex items-center justify-between px-3 py-3 rounded-lg text-sm font-medium transition-all group",
                                                activeCategory === cat.id
                                                    ? "bg-primary text-primary-foreground shadow-md shadow-primary/20 scale-[1.02]"
                                                    : "text-muted-foreground hover:bg-primary/10 hover:text-primary"
                                            )}
                                        >
                                            <span className="truncate">{cat.name}</span>
                                            <Badge
                                                variant={activeCategory === cat.id ? "secondary" : "outline"}
                                                className={cn(
                                                    "ml-2 shrink-0 border-none px-1.5 h-5 min-w-5 flex items-center justify-center rounded-full text-[10px]",
                                                    activeCategory === cat.id ? "bg-white/20 text-white" : "bg-muted text-muted-foreground"
                                                )}
                                            >
                                                {cat.count}
                                            </Badge>
                                        </button>
                                    ))}
                                </div>
                            </ScrollArea>
                        </div>

                        {/* Content: Permissions */}
                        <div className="flex-1 flex flex-col bg-background">
                            <div className="p-4 border-b bg-muted/5 flex items-center justify-between shrink-0">
                                <h3 className="font-bold flex items-center gap-2">
                                    <Lock className="size-4 text-primary" />
                                    {permissionCategories.find(c => c.id === activeCategory)?.name}
                                </h3>
                                <div className="flex items-center gap-2 px-3 py-1 bg-primary/5 rounded-full border border-primary/10">
                                    <Checkbox id="cat-all" />
                                    <label htmlFor="cat-all" className="text-xs font-bold text-primary uppercase cursor-pointer">
                                        Selecionar todos neste grupo
                                    </label>
                                </div>
                            </div>

                            <ScrollArea className="flex-1">
                                <div className="p-8 space-y-10">
                                    {permissionsByCategory[activeCategory]?.map((perm) => (
                                        <div key={perm.id} className="space-y-4 animate-in fade-in slide-in-from-bottom-2 duration-300">
                                            <div className="flex items-center gap-3 pb-2 border-b border-dashed">
                                                <Checkbox id={perm.id} className="size-5 rounded" />
                                                <label htmlFor={perm.id} className="text-lg font-bold tracking-tight cursor-pointer">
                                                    {perm.name}
                                                </label>
                                            </div>
                                            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-y-4 gap-x-6 pl-8">
                                                {perm.children.map((child: string, idx: number) => (
                                                    <div key={idx} className="flex items-center gap-3 group">
                                                        <Checkbox id={`${perm.id}-${idx}`} className="size-4 rounded-sm border-2 border-muted-foreground/30 data-[state=checked]:border-primary" />
                                                        <label
                                                            htmlFor={`${perm.id}-${idx}`}
                                                            className="text-sm font-medium text-muted-foreground group-hover:text-foreground transition-colors cursor-pointer"
                                                        >
                                                            {child}
                                                        </label>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                    )) || (
                                            <div className="flex flex-col items-center justify-center h-64 text-center space-y-4 opacity-50">
                                                <div className="size-16 rounded-full bg-muted flex items-center justify-center">
                                                    <Eye className="size-8 text-muted-foreground" />
                                                </div>
                                                <div className="space-y-1 text-sm">
                                                    <p className="font-bold">Nenhuma permissão detalhada</p>
                                                    <p>Este grupo possui apenas a permissão base ativada.</p>
                                                </div>
                                            </div>
                                        )}
                                </div>
                            </ScrollArea>
                        </div>
                    </div>

                    <DialogFooter className="p-6 bg-muted/30 border-t shrink-0">
                        <div className="flex items-center justify-between w-full">
                            <p className="text-xs text-muted-foreground font-medium">
                                * As alterações serão aplicadas imediatamente a todos os membros deste grupo.
                            </p>
                            <div className="flex gap-3">
                                <Button variant="outline" onClick={() => setIsPermissionsOpen(false)} className="px-8 font-semibold">
                                    Cancelar
                                </Button>
                                <Button className="px-10 font-bold shadow-lg shadow-primary/20" onClick={() => setIsPermissionsOpen(false)}>
                                    <CheckCircle2 className="mr-2 size-4" />
                                    Salvar Alterações
                                </Button>
                            </div>
                        </div>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </AppShell>
    );
}
