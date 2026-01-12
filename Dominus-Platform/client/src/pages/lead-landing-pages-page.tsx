import { useState, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Switch } from "@/components/ui/switch";
import { Label } from "@/components/ui/label";
import { useToast } from "@/hooks/use-toast";
import { ScrollArea } from "@/components/ui/scroll-area";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { DeleteModal } from "@/components/modals/delete-modal";
import {
  Plus,
  MoreVertical,
  Pencil,
  Trash2,
  Globe,
  Copy,
  ExternalLink,
  Eye,
} from "lucide-react";

function slugify(text: string): string {
  return text
    .toLowerCase()
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)+/g, "");
}

interface LandingPageFormData {
  title: string;
  subtitle: string;
  description: string;
  slug: string;
  formId: string | null;
  workflowId: string | null;
  primaryColor: string;
  backgroundColor: string;
  logoUrl: string;
  showSocialProof: boolean;
  socialProofText: string;
  footerText: string;
  isActive: boolean;
}

const defaultFormData: LandingPageFormData = {
  title: "",
  subtitle: "",
  description: "",
  slug: "",
  formId: null,
  workflowId: null,
  primaryColor: "#6366f1",
  backgroundColor: "#ffffff",
  logoUrl: "",
  showSocialProof: false,
  socialProofText: "",
  footerText: "Powered by Dominus",
  isActive: true,
};

function LandingPagePreview({ data }: { data: LandingPageFormData }) {
  return (
    <div 
      className="h-full rounded-lg border overflow-hidden"
      style={{ backgroundColor: data.backgroundColor }}
    >
      <div className="h-full flex flex-col">
        <div className="flex-1 flex flex-col items-center justify-center p-6 text-center">
          {data.logoUrl && (
            <img 
              src={data.logoUrl} 
              alt="Logo" 
              className="max-h-16 mb-6 object-contain"
              onError={(e) => {
                e.currentTarget.style.display = 'none';
              }}
            />
          )}
          
          <h1 
            className="text-2xl font-bold mb-2"
            style={{ color: data.primaryColor }}
          >
            {data.title || "Título da Landing Page"}
          </h1>
          
          {data.subtitle && (
            <p className="text-lg text-gray-600 mb-4">
              {data.subtitle}
            </p>
          )}
          
          {data.description && (
            <p className="text-sm text-gray-500 max-w-md mb-6">
              {data.description}
            </p>
          )}
          
          <div className="w-full max-w-sm space-y-3 mb-6">
            <div className="h-10 bg-gray-100 rounded-md border" />
            <div className="h-10 bg-gray-100 rounded-md border" />
            <div className="h-10 bg-gray-100 rounded-md border" />
            <button
              className="w-full h-10 rounded-md text-white font-medium"
              style={{ backgroundColor: data.primaryColor }}
            >
              Enviar
            </button>
          </div>
          
          {data.showSocialProof && data.socialProofText && (
            <p className="text-xs text-gray-400 italic">
              {data.socialProofText}
            </p>
          )}
        </div>
        
        {data.footerText && (
          <div className="p-4 border-t text-center">
            <p className="text-xs text-gray-400">
              {data.footerText}
            </p>
          </div>
        )}
      </div>
    </div>
  );
}

export default function LeadLandingPagesPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [modalOpen, setModalOpen] = useState(false);
  const [editingPage, setEditingPage] = useState<any>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [pageToDelete, setPageToDelete] = useState<any>(null);
  const [formData, setFormData] = useState<LandingPageFormData>(defaultFormData);
  const [slugManuallyEdited, setSlugManuallyEdited] = useState(false);

  const { data: landingPages = [], isLoading } = useQuery({
    queryKey: ["lead-landing-pages", currentWorkspace?.id],
    queryFn: () => api.getLeadLandingPages(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: forms = [] } = useQuery({
    queryKey: ["lead-forms", currentWorkspace?.id],
    queryFn: () => api.getLeadForms(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: workflows = [] } = useQuery({
    queryKey: ["lead-workflows", currentWorkspace?.id],
    queryFn: () => api.getLeadWorkflows(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createLeadLandingPage(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-landing-pages", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Landing page criada com sucesso!" });
      closeModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) =>
      api.updateLeadLandingPage(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-landing-pages", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Landing page atualizada com sucesso!" });
      closeModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.deleteLeadLandingPage(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lead-landing-pages", currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Landing page excluída com sucesso!" });
      setDeleteModalOpen(false);
      setPageToDelete(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const closeModal = () => {
    setModalOpen(false);
    setEditingPage(null);
    setFormData(defaultFormData);
    setSlugManuallyEdited(false);
  };

  const handleNewPage = () => {
    setEditingPage(null);
    setFormData(defaultFormData);
    setSlugManuallyEdited(false);
    setModalOpen(true);
  };

  const handleEditPage = (page: any) => {
    setEditingPage(page);
    setFormData({
      title: page.title || "",
      subtitle: page.subtitle || "",
      description: page.description || "",
      slug: page.slug || "",
      formId: page.formId || null,
      workflowId: page.workflowId || null,
      primaryColor: page.primaryColor || "#6366f1",
      backgroundColor: page.backgroundColor || "#ffffff",
      logoUrl: page.logoUrl || "",
      showSocialProof: page.showSocialProof || false,
      socialProofText: page.socialProofText || "",
      footerText: page.footerText || "Powered by Dominus",
      isActive: page.isActive ?? true,
    });
    setSlugManuallyEdited(true);
    setModalOpen(true);
  };

  const handleDeletePage = (page: any) => {
    setPageToDelete(page);
    setDeleteModalOpen(true);
  };

  const handleSubmit = () => {
    if (!formData.title.trim()) {
      toast({ title: "Erro", description: "O título é obrigatório", variant: "destructive" });
      return;
    }
    if (!formData.slug.trim()) {
      toast({ title: "Erro", description: "O slug é obrigatório", variant: "destructive" });
      return;
    }

    const dataToSubmit = {
      ...formData,
      formId: formData.formId || null,
      workflowId: formData.workflowId || null,
    };

    if (editingPage) {
      updateMutation.mutate({ id: editingPage.id, data: dataToSubmit });
    } else {
      createMutation.mutate(dataToSubmit);
    }
  };

  const handleTitleChange = (title: string) => {
    setFormData(prev => ({
      ...prev,
      title,
      slug: slugManuallyEdited ? prev.slug : slugify(title),
    }));
  };

  const handleSlugChange = (slug: string) => {
    setSlugManuallyEdited(true);
    setFormData(prev => ({
      ...prev,
      slug: slugify(slug),
    }));
  };

  const copyPublicLink = (slug: string) => {
    const link = `${window.location.origin}/l/${slug}`;
    navigator.clipboard.writeText(link);
    toast({ title: "Link copiado!", description: link });
  };

  const getFormName = (formId: string | null) => {
    if (!formId) return "-";
    const form = forms.find((f: any) => f.id === formId);
    return form?.name || "-";
  };

  const getWorkflowName = (workflowId: string | null) => {
    if (!workflowId) return "-";
    const workflow = workflows.find((w: any) => w.id === workflowId);
    return workflow?.name || "-";
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Landing Pages</h1>
            <p className="text-muted-foreground">
              Crie páginas de captura personalizadas para seus formulários.
            </p>
          </div>
          <Button className="gap-2" onClick={handleNewPage} data-testid="button-add-landing-page">
            <Plus className="size-4" />
            Nova Landing Page
          </Button>
        </div>

        {isLoading ? (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[...Array(3)].map((_, i) => (
              <Skeleton key={i} className="h-48 rounded-xl" />
            ))}
          </div>
        ) : landingPages.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-16 text-center">
            <div className="size-16 rounded-full bg-muted flex items-center justify-center mb-4">
              <Globe className="size-8 text-muted-foreground" />
            </div>
            <h3 className="text-lg font-semibold mb-2">Nenhuma landing page encontrada</h3>
            <p className="text-muted-foreground mb-4">
              Crie sua primeira landing page para começar a captar leads.
            </p>
            <Button onClick={handleNewPage}>
              <Plus className="size-4 mr-2" />
              Criar Landing Page
            </Button>
          </div>
        ) : (
          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {landingPages.map((page: any) => (
              <TactileCard
                key={page.id}
                className="hover:border-primary/50 transition-colors"
                data-testid={`card-landing-page-${page.id}`}
              >
                <div className="p-6">
                  <div className="flex items-start justify-between mb-4">
                    <div className="flex-1">
                      <div className="flex items-center gap-2 mb-1">
                        <h3
                          className="font-semibold text-lg"
                          data-testid={`text-landing-page-title-${page.id}`}
                        >
                          {page.title}
                        </h3>
                        <Badge variant={page.isActive ? "default" : "secondary"}>
                          {page.isActive ? "Ativo" : "Inativo"}
                        </Badge>
                      </div>
                      <p className="text-sm text-muted-foreground font-mono">
                        /l/{page.slug}
                      </p>
                    </div>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon" className="size-8">
                          <MoreVertical className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => handleEditPage(page)}>
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => copyPublicLink(page.slug)}>
                          <Copy className="size-4 mr-2" />
                          Copiar Link
                        </DropdownMenuItem>
                        <DropdownMenuItem asChild>
                          <a
                            href={`/l/${page.slug}`}
                            target="_blank"
                            rel="noopener noreferrer"
                          >
                            <ExternalLink className="size-4 mr-2" />
                            Abrir Página
                          </a>
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          onClick={() => handleDeletePage(page)}
                          className="text-destructive"
                        >
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>

                  <div className="space-y-2 text-sm">
                    <div className="flex items-center justify-between">
                      <span className="text-muted-foreground">Formulário:</span>
                      <span className="font-medium">{getFormName(page.formId)}</span>
                    </div>
                    <div className="flex items-center justify-between">
                      <span className="text-muted-foreground">Funil:</span>
                      <span className="font-medium">{getWorkflowName(page.workflowId)}</span>
                    </div>
                    <div className="flex items-center gap-2 pt-2">
                      <div
                        className="size-4 rounded-full border"
                        style={{ backgroundColor: page.primaryColor }}
                        title="Cor primária"
                      />
                      <div
                        className="size-4 rounded-full border"
                        style={{ backgroundColor: page.backgroundColor }}
                        title="Cor de fundo"
                      />
                    </div>
                  </div>

                  <div className="flex gap-2 mt-4 pt-4 border-t">
                    <Button
                      variant="outline"
                      size="sm"
                      className="flex-1 gap-1"
                      onClick={() => copyPublicLink(page.slug)}
                      data-testid={`button-copy-link-${page.id}`}
                    >
                      <Copy className="size-3" />
                      Copiar Link
                    </Button>
                    <Button
                      variant="outline"
                      size="sm"
                      className="flex-1 gap-1"
                      onClick={() => handleEditPage(page)}
                      data-testid={`button-edit-${page.id}`}
                    >
                      <Pencil className="size-3" />
                      Editar
                    </Button>
                  </div>
                </div>
              </TactileCard>
            ))}
          </div>
        )}
      </div>

      <Dialog open={modalOpen} onOpenChange={setModalOpen}>
        <DialogContent className="max-w-6xl h-[90vh]">
          <DialogHeader>
            <DialogTitle>
              {editingPage ? "Editar Landing Page" : "Nova Landing Page"}
            </DialogTitle>
          </DialogHeader>
          
          <div className="flex gap-6 h-full overflow-hidden">
            <ScrollArea className="flex-1 pr-4">
              <div className="space-y-6 pb-6">
                <div className="space-y-4">
                  <h3 className="font-medium text-sm text-muted-foreground uppercase tracking-wide">
                    Informações Básicas
                  </h3>
                  
                  <div className="space-y-2">
                    <Label htmlFor="title">Título *</Label>
                    <Input
                      id="title"
                      value={formData.title}
                      onChange={(e) => handleTitleChange(e.target.value)}
                      placeholder="Ex: Cadastre-se na nossa lista de espera"
                      data-testid="input-title"
                    />
                  </div>
                  
                  <div className="space-y-2">
                    <Label htmlFor="slug">Slug *</Label>
                    <div className="flex items-center gap-2">
                      <span className="text-sm text-muted-foreground">/l/</span>
                      <Input
                        id="slug"
                        value={formData.slug}
                        onChange={(e) => handleSlugChange(e.target.value)}
                        placeholder="minha-landing-page"
                        data-testid="input-slug"
                      />
                    </div>
                  </div>
                  
                  <div className="space-y-2">
                    <Label htmlFor="subtitle">Subtítulo</Label>
                    <Input
                      id="subtitle"
                      value={formData.subtitle}
                      onChange={(e) => setFormData(prev => ({ ...prev, subtitle: e.target.value }))}
                      placeholder="Uma frase curta que complementa o título"
                      data-testid="input-subtitle"
                    />
                  </div>
                  
                  <div className="space-y-2">
                    <Label htmlFor="description">Descrição</Label>
                    <Textarea
                      id="description"
                      value={formData.description}
                      onChange={(e) => setFormData(prev => ({ ...prev, description: e.target.value }))}
                      placeholder="Uma descrição mais detalhada sobre sua oferta"
                      rows={3}
                      data-testid="input-description"
                    />
                  </div>
                </div>

                <div className="space-y-4">
                  <h3 className="font-medium text-sm text-muted-foreground uppercase tracking-wide">
                    Vinculações
                  </h3>
                  
                  <div className="space-y-2">
                    <Label htmlFor="formId">Formulário</Label>
                    <Select
                      value={formData.formId || "none"}
                      onValueChange={(v) => setFormData(prev => ({ ...prev, formId: v === "none" ? null : v }))}
                    >
                      <SelectTrigger data-testid="select-form">
                        <SelectValue placeholder="Selecione um formulário" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="none">Nenhum</SelectItem>
                        {forms.map((form: any) => (
                          <SelectItem key={form.id} value={form.id}>
                            {form.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                  
                  <div className="space-y-2">
                    <Label htmlFor="workflowId">Funil/Workflow</Label>
                    <Select
                      value={formData.workflowId || "none"}
                      onValueChange={(v) => setFormData(prev => ({ ...prev, workflowId: v === "none" ? null : v }))}
                    >
                      <SelectTrigger data-testid="select-workflow">
                        <SelectValue placeholder="Selecione um funil" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="none">Nenhum</SelectItem>
                        {workflows.map((workflow: any) => (
                          <SelectItem key={workflow.id} value={workflow.id}>
                            {workflow.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                </div>

                <div className="space-y-4">
                  <h3 className="font-medium text-sm text-muted-foreground uppercase tracking-wide">
                    Aparência
                  </h3>
                  
                  <div className="space-y-2">
                    <Label htmlFor="logoUrl">URL do Logo</Label>
                    <Input
                      id="logoUrl"
                      type="url"
                      value={formData.logoUrl}
                      onChange={(e) => setFormData(prev => ({ ...prev, logoUrl: e.target.value }))}
                      placeholder="https://exemplo.com/logo.png"
                      data-testid="input-logo-url"
                    />
                  </div>
                  
                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="primaryColor">Cor Primária</Label>
                      <div className="flex items-center gap-2">
                        <input
                          type="color"
                          id="primaryColor"
                          value={formData.primaryColor}
                          onChange={(e) => setFormData(prev => ({ ...prev, primaryColor: e.target.value }))}
                          className="w-10 h-10 rounded border cursor-pointer"
                          data-testid="input-primary-color"
                        />
                        <Input
                          value={formData.primaryColor}
                          onChange={(e) => setFormData(prev => ({ ...prev, primaryColor: e.target.value }))}
                          className="flex-1"
                        />
                      </div>
                    </div>
                    
                    <div className="space-y-2">
                      <Label htmlFor="backgroundColor">Cor de Fundo</Label>
                      <div className="flex items-center gap-2">
                        <input
                          type="color"
                          id="backgroundColor"
                          value={formData.backgroundColor}
                          onChange={(e) => setFormData(prev => ({ ...prev, backgroundColor: e.target.value }))}
                          className="w-10 h-10 rounded border cursor-pointer"
                          data-testid="input-background-color"
                        />
                        <Input
                          value={formData.backgroundColor}
                          onChange={(e) => setFormData(prev => ({ ...prev, backgroundColor: e.target.value }))}
                          className="flex-1"
                        />
                      </div>
                    </div>
                  </div>
                </div>

                <div className="space-y-4">
                  <h3 className="font-medium text-sm text-muted-foreground uppercase tracking-wide">
                    Extras
                  </h3>
                  
                  <div className="flex items-center justify-between">
                    <div>
                      <Label htmlFor="showSocialProof">Mostrar Social Proof</Label>
                      <p className="text-xs text-muted-foreground">
                        Exibir texto de prova social na página
                      </p>
                    </div>
                    <Switch
                      id="showSocialProof"
                      checked={formData.showSocialProof}
                      onCheckedChange={(checked) => setFormData(prev => ({ ...prev, showSocialProof: checked }))}
                      data-testid="switch-social-proof"
                    />
                  </div>
                  
                  {formData.showSocialProof && (
                    <div className="space-y-2">
                      <Label htmlFor="socialProofText">Texto do Social Proof</Label>
                      <Input
                        id="socialProofText"
                        value={formData.socialProofText}
                        onChange={(e) => setFormData(prev => ({ ...prev, socialProofText: e.target.value }))}
                        placeholder="Ex: Mais de 1.000 pessoas já se cadastraram!"
                        data-testid="input-social-proof-text"
                      />
                    </div>
                  )}
                  
                  <div className="space-y-2">
                    <Label htmlFor="footerText">Texto do Rodapé</Label>
                    <Input
                      id="footerText"
                      value={formData.footerText}
                      onChange={(e) => setFormData(prev => ({ ...prev, footerText: e.target.value }))}
                      placeholder="Powered by Dominus"
                      data-testid="input-footer-text"
                    />
                  </div>
                  
                  <div className="flex items-center justify-between">
                    <div>
                      <Label htmlFor="isActive">Página Ativa</Label>
                      <p className="text-xs text-muted-foreground">
                        Desative para esconder a página do público
                      </p>
                    </div>
                    <Switch
                      id="isActive"
                      checked={formData.isActive}
                      onCheckedChange={(checked) => setFormData(prev => ({ ...prev, isActive: checked }))}
                      data-testid="switch-is-active"
                    />
                  </div>
                </div>

                <div className="flex gap-3 pt-4 border-t">
                  <Button
                    variant="outline"
                    onClick={closeModal}
                    className="flex-1"
                  >
                    Cancelar
                  </Button>
                  <Button
                    onClick={handleSubmit}
                    className="flex-1"
                    disabled={createMutation.isPending || updateMutation.isPending}
                    data-testid="button-save"
                  >
                    {createMutation.isPending || updateMutation.isPending
                      ? "Salvando..."
                      : editingPage
                      ? "Salvar Alterações"
                      : "Criar Landing Page"}
                  </Button>
                </div>
              </div>
            </ScrollArea>

            <div className="w-80 flex flex-col">
              <div className="flex items-center gap-2 mb-3">
                <Eye className="size-4 text-muted-foreground" />
                <span className="text-sm font-medium">Preview</span>
              </div>
              <div className="flex-1 rounded-lg border bg-muted/30 overflow-hidden">
                <LandingPagePreview data={formData} />
              </div>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => pageToDelete && deleteMutation.mutate(pageToDelete.id)}
        title="Excluir Landing Page"
        description={`Tem certeza que deseja excluir a landing page "${pageToDelete?.title}"? Esta ação não pode ser desfeita.`}
        isLoading={deleteMutation.isPending}
      />
    </AppShell>
  );
}
