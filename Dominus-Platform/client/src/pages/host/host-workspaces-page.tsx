import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { HostLayout } from "@/components/layout/host-layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Input } from "@/components/ui/input";
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from "@/components/ui/table";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Label } from "@/components/ui/label";
import { 
  Building2, 
  Search, 
  Users, 
  Briefcase,
  Settings,
  ExternalLink,
  Loader2,
  Crown
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { toast } from "sonner";

interface WorkspaceWithDetails {
  id: string;
  name: string;
  slug: string;
  ownerId: string;
  ownerName: string;
  ownerEmail: string;
  memberCount: number;
  createdAt: string;
  subscription?: {
    plan: string;
    status: string;
    maxUsers: number;
    maxProjects: number;
    trialEndsAt?: string;
  };
}

export default function HostWorkspacesPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedWorkspace, setSelectedWorkspace] = useState<WorkspaceWithDetails | null>(null);
  const [subscriptionDialogOpen, setSubscriptionDialogOpen] = useState(false);
  const queryClient = useQueryClient();

  const { data: workspaces = [], isLoading } = useQuery<WorkspaceWithDetails[]>({
    queryKey: ["host", "workspaces"],
    queryFn: api.getHostWorkspaces,
  });

  const updateSubscriptionMutation = useMutation({
    mutationFn: ({ workspaceId, data }: { workspaceId: string; data: any }) =>
      api.updateWorkspaceSubscription(workspaceId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["host", "workspaces"] });
      toast.success("Assinatura atualizada com sucesso");
      setSubscriptionDialogOpen(false);
      setSelectedWorkspace(null);
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao atualizar assinatura");
    },
  });

  const filteredWorkspaces = workspaces.filter(
    (w) =>
      w.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      w.slug.toLowerCase().includes(searchTerm.toLowerCase()) ||
      w.ownerEmail.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const planColors: Record<string, string> = {
    TRIAL: "bg-gray-500",
    STARTER: "bg-blue-500",
    PROFESSIONAL: "bg-purple-500",
    ENTERPRISE: "bg-amber-500",
  };

  const statusColors: Record<string, string> = {
    ACTIVE: "bg-green-500",
    TRIAL: "bg-gray-500",
    SUSPENDED: "bg-red-500",
    CANCELLED: "bg-red-700",
    EXPIRED: "bg-orange-500",
  };

  const handleEditSubscription = (workspace: WorkspaceWithDetails) => {
    setSelectedWorkspace(workspace);
    setSubscriptionDialogOpen(true);
  };

  const handleSaveSubscription = (formData: any) => {
    if (!selectedWorkspace) return;
    updateSubscriptionMutation.mutate({
      workspaceId: selectedWorkspace.id,
      data: formData,
    });
  };

  if (isLoading) {
    return (
      <HostLayout>
        <div className="flex items-center justify-center h-full">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </HostLayout>
    );
  }

  return (
    <HostLayout>
      <div className="container mx-auto p-6 space-y-6">
        <div className="flex items-center gap-3 mb-8">
          <Building2 className="h-8 w-8 text-primary" />
          <div>
            <h1 className="text-3xl font-bold">Clientes (Workspaces)</h1>
            <p className="text-muted-foreground">Gerenciar workspaces da plataforma</p>
          </div>
        </div>

        <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle>Workspaces ({filteredWorkspaces.length})</CardTitle>
            <div className="relative w-72">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
              <Input
                placeholder="Buscar por nome, slug ou e-mail..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10"
                data-testid="input-search-workspaces"
              />
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Workspace</TableHead>
                <TableHead>Proprietário</TableHead>
                <TableHead className="text-center">Membros</TableHead>
                <TableHead>Plano</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Criado em</TableHead>
                <TableHead className="text-right">Ações</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filteredWorkspaces.map((workspace) => (
                <TableRow key={workspace.id} data-testid={`row-workspace-${workspace.id}`}>
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
                    <Badge className={planColors[workspace.subscription?.plan || 'TRIAL']}>
                      {workspace.subscription?.plan || 'TRIAL'}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    <Badge variant="outline" className={`border-0 text-white ${statusColors[workspace.subscription?.status || 'TRIAL']}`}>
                      {workspace.subscription?.status || 'TRIAL'}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    {format(new Date(workspace.createdAt), "dd/MM/yyyy", { locale: ptBR })}
                  </TableCell>
                  <TableCell className="text-right">
                    <div className="flex items-center justify-end gap-2">
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleEditSubscription(workspace)}
                        data-testid={`button-edit-subscription-${workspace.id}`}
                      >
                        <Settings className="h-4 w-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        asChild
                      >
                        <a href={`/${workspace.slug}/dashboard`} target="_blank" rel="noopener noreferrer">
                          <ExternalLink className="h-4 w-4" />
                        </a>
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
              {filteredWorkspaces.length === 0 && (
                <TableRow>
                  <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
                    Nenhum workspace encontrado
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

        <SubscriptionDialog
          open={subscriptionDialogOpen}
          onOpenChange={setSubscriptionDialogOpen}
          workspace={selectedWorkspace}
          onSave={handleSaveSubscription}
          isLoading={updateSubscriptionMutation.isPending}
        />
      </div>
    </HostLayout>
  );
}

function SubscriptionDialog({
  open,
  onOpenChange,
  workspace,
  onSave,
  isLoading,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  workspace: WorkspaceWithDetails | null;
  onSave: (data: any) => void;
  isLoading: boolean;
}) {
  const [plan, setPlan] = useState(workspace?.subscription?.plan || 'TRIAL');
  const [status, setStatus] = useState(workspace?.subscription?.status || 'TRIAL');
  const [maxUsers, setMaxUsers] = useState(workspace?.subscription?.maxUsers?.toString() || '3');
  const [maxProjects, setMaxProjects] = useState(workspace?.subscription?.maxProjects?.toString() || '5');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave({
      plan,
      status,
      maxUsers: parseInt(maxUsers),
      maxProjects: parseInt(maxProjects),
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <Crown className="h-5 w-5 text-amber-500" />
            Gerenciar Assinatura
          </DialogTitle>
          <DialogDescription>
            Configurar plano e limites para {workspace?.name}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="plan">Plano</Label>
            <Select value={plan} onValueChange={setPlan}>
              <SelectTrigger data-testid="select-plan">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="TRIAL">Trial</SelectItem>
                <SelectItem value="STARTER">Starter</SelectItem>
                <SelectItem value="PROFESSIONAL">Professional</SelectItem>
                <SelectItem value="ENTERPRISE">Enterprise</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="status">Status</Label>
            <Select value={status} onValueChange={setStatus}>
              <SelectTrigger data-testid="select-status">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="TRIAL">Trial</SelectItem>
                <SelectItem value="ACTIVE">Ativo</SelectItem>
                <SelectItem value="SUSPENDED">Suspenso</SelectItem>
                <SelectItem value="CANCELLED">Cancelado</SelectItem>
                <SelectItem value="EXPIRED">Expirado</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="maxUsers">Máximo de Usuários</Label>
              <Input
                id="maxUsers"
                type="number"
                value={maxUsers}
                onChange={(e) => setMaxUsers(e.target.value)}
                data-testid="input-max-users"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="maxProjects">Máximo de Projetos</Label>
              <Input
                id="maxProjects"
                type="number"
                value={maxProjects}
                onChange={(e) => setMaxProjects(e.target.value)}
                data-testid="input-max-projects"
              />
            </div>
          </div>

          <div className="flex justify-end gap-2 pt-4">
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading} data-testid="button-save-subscription">
              {isLoading && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
              Salvar
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
