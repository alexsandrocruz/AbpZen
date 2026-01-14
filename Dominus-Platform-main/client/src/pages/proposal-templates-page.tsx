import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useLocation } from "wouter";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { 
  Plus, 
  LayoutTemplate, 
  MoreHorizontal, 
  Pencil, 
  Trash2, 
  Copy,
  Globe,
  Building2,
} from "lucide-react";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";

export default function ProposalTemplatesPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [, navigate] = useLocation();

  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [selectedTemplate, setSelectedTemplate] = useState<any>(null);
  const [formData, setFormData] = useState({ name: "", description: "" });

  const { data: templates = [], isLoading } = useQuery({
    queryKey: ['proposalTemplates', currentWorkspace?.id],
    queryFn: () => api.getProposalTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createMutation = useMutation({
    mutationFn: async (data: { name: string; description: string }) => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({
          ...data,
          workspaceId: currentWorkspace!.id,
          isPublic: false,
          style: JSON.stringify({
            primaryColor: '#22c55e',
            secondaryColor: '#eab308',
            accentColor: '#3b82f6',
            fontFamily: 'Inter',
            headingFont: 'Inter',
            backgroundColor: '#ffffff',
          }),
        }),
      });
      if (!res.ok) throw new Error('Erro ao criar modelo');
      return res.json();
    },
    onSuccess: (newTemplate) => {
      queryClient.invalidateQueries({ queryKey: ['proposalTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Modelo criado com sucesso!" });
      setCreateModalOpen(false);
      setFormData({ name: "", description: "" });
      if (currentWorkspace?.slug) {
        navigate(`/${currentWorkspace.slug}/proposal-templates/${newTemplate.id}/builder`);
      }
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async (templateId: string) => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates/${templateId}`, {
        method: 'DELETE',
        credentials: 'include',
      });
      if (!res.ok) throw new Error('Erro ao excluir modelo');
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['proposalTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Modelo excluído com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedTemplate(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const duplicateMutation = useMutation({
    mutationFn: async (template: any) => {
      const res = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({
          name: `${template.name} (Cópia)`,
          description: template.description,
          workspaceId: currentWorkspace!.id,
          isPublic: false,
          style: template.style,
        }),
      });
      if (!res.ok) throw new Error('Erro ao duplicar modelo');
      const newTemplate = await res.json();
      
      const blocksRes = await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates/${template.id}`, {
        credentials: 'include',
      });
      if (blocksRes.ok) {
        const { blocks } = await blocksRes.json();
        if (blocks && blocks.length > 0) {
          for (const block of blocks) {
            await fetch(`/api/workspaces/${currentWorkspace!.id}/proposal-templates/${newTemplate.id}/blocks`, {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              credentials: 'include',
              body: JSON.stringify({
                blockType: block.blockType,
                position: block.position,
                content: block.content,
                style: block.style,
              }),
            });
          }
        }
      }
      return newTemplate;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['proposalTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Modelo duplicado com sucesso!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const systemTemplates = templates.filter((t: any) => t.isPublic);
  const workspaceTemplates = templates.filter((t: any) => !t.isPublic && t.workspaceId === currentWorkspace?.id);

  const handleEdit = (template: any) => {
    if (currentWorkspace?.slug) {
      navigate(`/${currentWorkspace.slug}/proposal-templates/${template.id}/builder`);
    }
  };

  const handleDelete = (template: any) => {
    setSelectedTemplate(template);
    setDeleteModalOpen(true);
  };

  const handleDuplicate = (template: any) => {
    duplicateMutation.mutate(template);
  };

  const handleCreate = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.name.trim()) return;
    createMutation.mutate(formData);
  };

  const parseStyle = (style: string) => {
    try {
      return JSON.parse(style);
    } catch {
      return {};
    }
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Modelos de Proposta</h1>
            <p className="text-muted-foreground">Crie e gerencie modelos reutilizáveis para suas propostas.</p>
          </div>
          <Button onClick={() => setCreateModalOpen(true)} className="gap-2" data-testid="button-new-template">
            <Plus className="size-4" />
            Novo Modelo
          </Button>
        </div>

        {workspaceTemplates.length > 0 && (
          <div className="space-y-4">
            <div className="flex items-center gap-2">
              <Building2 className="size-5 text-primary" />
              <h2 className="text-xl font-semibold">Meus Modelos</h2>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              {workspaceTemplates.map((template: any) => {
                const style = parseStyle(template.style);
                return (
                  <TactileCard 
                    key={template.id} 
                    className="p-4 cursor-pointer hover:border-primary transition-colors"
                    onClick={() => handleEdit(template)}
                    data-testid={`template-card-${template.id}`}
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex items-center gap-3">
                        <div 
                          className="size-10 rounded-lg flex items-center justify-center"
                          style={{ backgroundColor: style.primaryColor || '#22c55e' }}
                        >
                          <LayoutTemplate className="size-5 text-white" />
                        </div>
                        <div>
                          <h3 className="font-medium">{template.name}</h3>
                          {template.description && (
                            <p className="text-sm text-muted-foreground line-clamp-1">{template.description}</p>
                          )}
                        </div>
                      </div>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild onClick={(e) => e.stopPropagation()}>
                          <Button variant="ghost" size="icon" className="size-8">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={(e) => { e.stopPropagation(); handleEdit(template); }}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem onClick={(e) => { e.stopPropagation(); handleDuplicate(template); }}>
                            <Copy className="size-4 mr-2" />
                            Duplicar
                          </DropdownMenuItem>
                          <DropdownMenuItem 
                            onClick={(e) => { e.stopPropagation(); handleDelete(template); }}
                            className="text-destructive"
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                  </TactileCard>
                );
              })}
            </div>
          </div>
        )}

        {isLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {[...Array(6)].map((_, i) => (
              <Skeleton key={i} className="h-24 rounded-xl" />
            ))}
          </div>
        ) : workspaceTemplates.length === 0 && (
          <TactileCard className="p-12 text-center">
            <LayoutTemplate className="size-12 mx-auto text-muted-foreground mb-4" />
            <h3 className="text-lg font-medium mb-2">Nenhum modelo criado</h3>
            <p className="text-muted-foreground mb-4">Crie modelos personalizados para agilizar suas propostas.</p>
            <Button onClick={() => setCreateModalOpen(true)} className="gap-2">
              <Plus className="size-4" />
              Criar Primeiro Modelo
            </Button>
          </TactileCard>
        )}

        {systemTemplates.length > 0 && (
          <div className="space-y-4">
            <div className="flex items-center gap-2">
              <Globe className="size-5 text-muted-foreground" />
              <h2 className="text-xl font-semibold text-muted-foreground">Modelos do Sistema</h2>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              {systemTemplates.map((template: any) => {
                const style = parseStyle(template.style);
                return (
                  <TactileCard 
                    key={template.id} 
                    className="p-4 opacity-80"
                    data-testid={`system-template-card-${template.id}`}
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex items-center gap-3">
                        <div 
                          className="size-10 rounded-lg flex items-center justify-center"
                          style={{ backgroundColor: style.primaryColor || '#666' }}
                        >
                          <LayoutTemplate className="size-5 text-white" />
                        </div>
                        <div>
                          <h3 className="font-medium">{template.name}</h3>
                          {template.description && (
                            <p className="text-sm text-muted-foreground line-clamp-1">{template.description}</p>
                          )}
                        </div>
                      </div>
                      <Button 
                        variant="ghost" 
                        size="sm"
                        onClick={() => handleDuplicate(template)}
                        className="gap-1"
                      >
                        <Copy className="size-4" />
                        Usar
                      </Button>
                    </div>
                  </TactileCard>
                );
              })}
            </div>
          </div>
        )}
      </div>

      <Dialog open={createModalOpen} onOpenChange={setCreateModalOpen}>
        <DialogContent className="max-w-md">
          <DialogHeader>
            <DialogTitle>Novo Modelo de Proposta</DialogTitle>
          </DialogHeader>
          <form onSubmit={handleCreate} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="name">Nome do Modelo *</Label>
              <Input
                id="name"
                placeholder="Ex: Proposta para Sistemas"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
                data-testid="input-template-name"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="description">Descrição</Label>
              <Textarea
                id="description"
                placeholder="Descreva para que serve este modelo..."
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                rows={3}
                data-testid="input-template-description"
              />
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={() => setCreateModalOpen(false)}>
                Cancelar
              </Button>
              <Button type="submit" disabled={createMutation.isPending} data-testid="button-save-template">
                {createMutation.isPending ? "Criando..." : "Criar e Editar"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        title="Excluir Modelo"
        description={`Tem certeza que deseja excluir o modelo "${selectedTemplate?.name}"? Esta ação não pode ser desfeita.`}
        onConfirm={() => selectedTemplate && deleteMutation.mutate(selectedTemplate.id)}
        isLoading={deleteMutation.isPending}
      />
    </AppShell>
  );
}
