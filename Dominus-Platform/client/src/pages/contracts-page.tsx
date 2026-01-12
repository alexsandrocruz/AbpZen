import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { 
  Plus, 
  FileSignature, 
  MoreHorizontal, 
  Pencil, 
  Trash2, 
  FileText, 
  Link as LinkIcon,
  Copy,
  Check,
  ExternalLink
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { TactileCard } from "@/components/ui/tactile-card";
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
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { ClientCombobox } from "@/components/ui/client-combobox";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Loader2 } from "lucide-react";

const statusMap: Record<string, string> = {
  'DRAFT': 'Rascunho',
  'SENT': 'Enviado',
  'SIGNED': 'Assinado',
  'CANCELLED': 'Cancelado',
};

const statusColors: Record<string, string> = {
  'DRAFT': 'bg-yellow-50 text-yellow-700 border-yellow-200',
  'SENT': 'bg-blue-50 text-blue-700 border-blue-200',
  'SIGNED': 'bg-green-50 text-green-700 border-green-200',
  'CANCELLED': 'bg-red-50 text-red-700 border-red-200',
};

interface TemplateModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  template?: any;
}

function TemplateModal({ open, onOpenChange, template }: TemplateModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  
  const [formData, setFormData] = useState({
    name: template?.name || "",
    description: template?.description || "",
    content: template?.content || "",
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createContractTemplate(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contract-templates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template criado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateContractTemplate(currentWorkspace!.id, template.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contract-templates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template atualizado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (template) {
      updateMutation.mutate(formData);
    } else {
      createMutation.mutate(formData);
    }
  };

  const isSubmitting = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[700px] max-h-[85vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{template ? "Editar Template" : "Novo Template"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome *</Label>
            <Input
              id="name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="Ex: Contrato de Prestação de Serviços"
              required
              data-testid="input-template-name"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Input
              id="description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              placeholder="Descrição opcional do template"
              data-testid="input-template-description"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="content">Conteúdo *</Label>
            <p className="text-xs text-muted-foreground">
              Use variáveis como: {"{{cliente_nome}}"}, {"{{projeto_nome}}"}, {"{{valor}}"}, {"{{data}}"}
            </p>
            <Textarea
              id="content"
              value={formData.content}
              onChange={(e) => setFormData({ ...formData, content: e.target.value })}
              placeholder="Digite o conteúdo do contrato..."
              className="min-h-[300px] font-mono text-sm"
              required
              data-testid="textarea-template-content"
            />
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isSubmitting} data-testid="button-save-template">
              {isSubmitting && <Loader2 className="size-4 mr-2 animate-spin" />}
              {template ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

interface ContractModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  contract?: any;
}

function ContractModal({ open, onOpenChange, contract }: ContractModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  
  const [formData, setFormData] = useState({
    title: contract?.title || "",
    clientId: contract?.clientId || "",
    projectId: contract?.projectId || "",
    proposalId: contract?.proposalId || "",
    templateId: contract?.templateId || "",
    content: contract?.content || "",
    status: contract?.status || "DRAFT",
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: projects = [] } = useQuery({
    queryKey: ['projects', currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: proposals = [] } = useQuery({
    queryKey: ['proposals', currentWorkspace?.id],
    queryFn: () => api.getProposals(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: templates = [] } = useQuery({
    queryKey: ['contract-templates', currentWorkspace?.id],
    queryFn: () => api.getContractTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createContract(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contracts', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Contrato criado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateContract(currentWorkspace!.id, contract.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contracts', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Contrato atualizado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleTemplateChange = (templateId: string) => {
    const selectedTemplate = templates.find((t: any) => t.id === templateId);
    setFormData({ 
      ...formData, 
      templateId,
      content: selectedTemplate?.content || formData.content 
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const data = {
      ...formData,
      projectId: formData.projectId || null,
      proposalId: formData.proposalId || null,
      templateId: formData.templateId || null,
    };
    if (contract) {
      updateMutation.mutate(data);
    } else {
      createMutation.mutate(data);
    }
  };

  const isSubmitting = createMutation.isPending || updateMutation.isPending;

  const filteredProjects = formData.clientId 
    ? projects.filter((p: any) => p.clientId === formData.clientId)
    : projects;

  const filteredProposals = formData.clientId 
    ? proposals.filter((p: any) => p.clientId === formData.clientId)
    : proposals;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[800px] max-h-[85vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{contract ? "Editar Contrato" : "Novo Contrato"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="title">Título *</Label>
              <Input
                id="title"
                value={formData.title}
                onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                placeholder="Ex: Contrato de Desenvolvimento Web"
                required
                data-testid="input-contract-title"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="status">Status</Label>
              <Select 
                value={formData.status} 
                onValueChange={(value) => setFormData({ ...formData, status: value })}
              >
                <SelectTrigger data-testid="select-contract-status">
                  <SelectValue placeholder="Selecione o status" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="DRAFT">Rascunho</SelectItem>
                  <SelectItem value="SENT">Enviado</SelectItem>
                  <SelectItem value="SIGNED">Assinado</SelectItem>
                  <SelectItem value="CANCELLED">Cancelado</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="clientId">Cliente *</Label>
              <ClientCombobox
                clients={clients}
                value={formData.clientId}
                onChange={(value) => setFormData({ ...formData, clientId: value, projectId: "", proposalId: "" })}
                workspaceId={currentWorkspace?.id || ""}
                placeholder="Buscar ou criar cliente..."
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="templateId">Template</Label>
              <Select 
                value={formData.templateId} 
                onValueChange={handleTemplateChange}
              >
                <SelectTrigger data-testid="select-contract-template">
                  <SelectValue placeholder="Usar template (opcional)" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">Nenhum</SelectItem>
                  {templates.map((template: any) => (
                    <SelectItem key={template.id} value={template.id}>
                      {template.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="projectId">Projeto</Label>
              <Select 
                value={formData.projectId} 
                onValueChange={(value) => setFormData({ ...formData, projectId: value })}
                disabled={!formData.clientId}
              >
                <SelectTrigger data-testid="select-contract-project">
                  <SelectValue placeholder="Vincular projeto (opcional)" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">Nenhum</SelectItem>
                  {filteredProjects.map((project: any) => (
                    <SelectItem key={project.id} value={project.id}>
                      {project.title}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="proposalId">Proposta</Label>
              <Select 
                value={formData.proposalId} 
                onValueChange={(value) => setFormData({ ...formData, proposalId: value })}
                disabled={!formData.clientId}
              >
                <SelectTrigger data-testid="select-contract-proposal">
                  <SelectValue placeholder="Vincular proposta (opcional)" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">Nenhuma</SelectItem>
                  {filteredProposals.map((proposal: any) => (
                    <SelectItem key={proposal.id} value={proposal.id}>
                      {proposal.title}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="content">Conteúdo *</Label>
            <Textarea
              id="content"
              value={formData.content}
              onChange={(e) => setFormData({ ...formData, content: e.target.value })}
              placeholder="Digite o conteúdo do contrato..."
              className="min-h-[250px] font-mono text-sm"
              required
              data-testid="textarea-contract-content"
            />
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isSubmitting || !formData.clientId} data-testid="button-save-contract">
              {isSubmitting && <Loader2 className="size-4 mr-2 animate-spin" />}
              {contract ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

export default function ContractsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [activeTab, setActiveTab] = useState("contracts");
  const [templateModalOpen, setTemplateModalOpen] = useState(false);
  const [contractModalOpen, setContractModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedTemplate, setSelectedTemplate] = useState<any>(null);
  const [selectedContract, setSelectedContract] = useState<any>(null);
  const [deleteType, setDeleteType] = useState<'template' | 'contract'>('contract');
  const [copiedId, setCopiedId] = useState<string | null>(null);

  const { data: templates = [], isLoading: templatesLoading } = useQuery({
    queryKey: ['contract-templates', currentWorkspace?.id],
    queryFn: () => api.getContractTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: contracts = [], isLoading: contractsLoading } = useQuery({
    queryKey: ['contracts', currentWorkspace?.id],
    queryFn: () => api.getContracts(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const deleteTemplateMutation = useMutation({
    mutationFn: (templateId: string) => api.deleteContractTemplate(currentWorkspace!.id, templateId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contract-templates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template excluído com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedTemplate(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteContractMutation = useMutation({
    mutationFn: (contractId: string) => api.deleteContract(currentWorkspace!.id, contractId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['contracts', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Contrato excluído com sucesso!" });
      setDeleteModalOpen(false);
      setSelectedContract(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const generateLinkMutation = useMutation({
    mutationFn: (contractId: string) => api.generateContractLink(currentWorkspace!.id, contractId),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ['contracts', currentWorkspace?.id] });
      const fullUrl = `${window.location.origin}${data.url}`;
      navigator.clipboard.writeText(fullUrl);
      toast({ 
        title: "Link gerado!", 
        description: "Link copiado para a área de transferência." 
      });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const getClientName = (clientId: string) => {
    const client = clients.find((c: any) => c.id === clientId);
    return client?.companyName || client?.name || 'Cliente não encontrado';
  };

  const handleEditTemplate = (template: any) => {
    setSelectedTemplate(template);
    setTemplateModalOpen(true);
  };

  const handleDeleteTemplate = (template: any) => {
    setSelectedTemplate(template);
    setDeleteType('template');
    setDeleteModalOpen(true);
  };

  const handleNewTemplate = () => {
    setSelectedTemplate(null);
    setTemplateModalOpen(true);
  };

  const handleEditContract = (contract: any) => {
    setSelectedContract(contract);
    setContractModalOpen(true);
  };

  const handleDeleteContract = (contract: any) => {
    setSelectedContract(contract);
    setDeleteType('contract');
    setDeleteModalOpen(true);
  };

  const handleNewContract = () => {
    setSelectedContract(null);
    setContractModalOpen(true);
  };

  const handleCopyLink = (contract: any) => {
    const fullUrl = `${window.location.origin}/c/${contract.publicToken}`;
    navigator.clipboard.writeText(fullUrl);
    setCopiedId(contract.id);
    toast({ title: "Link copiado!" });
    setTimeout(() => setCopiedId(null), 2000);
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Contratos</h1>
            <p className="text-muted-foreground">Gerencie templates e contratos com assinaturas digitais.</p>
          </div>
        </div>

        <Tabs value={activeTab} onValueChange={setActiveTab}>
          <div className="flex items-center justify-between">
            <TabsList>
              <TabsTrigger value="contracts" data-testid="tab-contracts">Contratos</TabsTrigger>
              <TabsTrigger value="templates" data-testid="tab-templates">Templates</TabsTrigger>
            </TabsList>
            {activeTab === "templates" ? (
              <Button className="gap-2" onClick={handleNewTemplate} data-testid="button-add-template">
                <Plus className="size-4" />
                Novo Template
              </Button>
            ) : (
              <Button className="gap-2" onClick={handleNewContract} data-testid="button-add-contract">
                <Plus className="size-4" />
                Novo Contrato
              </Button>
            )}
          </div>

          <TabsContent value="templates" className="mt-6">
            {templatesLoading ? (
              <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                {[...Array(3)].map((_, i) => (
                  <Skeleton key={i} className="h-36 rounded-xl" />
                ))}
              </div>
            ) : templates.length > 0 ? (
              <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                {templates.map((template: any) => (
                  <TactileCard key={template.id} hover className="p-6 space-y-3 relative group" data-testid={`card-template-${template.id}`}>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon" className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity">
                          <MoreHorizontal className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => handleEditTemplate(template)}>
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => handleDeleteTemplate(template)} className="text-destructive">
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>

                    <div className="flex items-start gap-3">
                      <div className="size-10 rounded-lg bg-primary/10 flex items-center justify-center">
                        <FileText className="size-5 text-primary" />
                      </div>
                      <div className="flex-1 min-w-0">
                        <h3 className="font-semibold truncate">{template.name}</h3>
                        {template.description && (
                          <p className="text-sm text-muted-foreground line-clamp-1">{template.description}</p>
                        )}
                      </div>
                    </div>

                    <p className="text-sm text-muted-foreground line-clamp-2">
                      {template.content.substring(0, 150)}...
                    </p>

                    <div className="text-xs text-muted-foreground">
                      Criado em {format(new Date(template.createdAt), "d 'de' MMM, yyyy", { locale: ptBR })}
                    </div>
                  </TactileCard>
                ))}
              </div>
            ) : (
              <TactileCard className="min-h-[400px] flex flex-col items-center justify-center text-center p-8 space-y-6 border-dashed">
                <div className="size-20 rounded-full bg-primary/5 flex items-center justify-center">
                  <FileText className="size-10 text-primary/40" />
                </div>
                <div className="max-w-md space-y-2">
                  <h3 className="text-xl font-semibold">Nenhum template ainda</h3>
                  <p className="text-muted-foreground">Crie templates de contrato reutilizáveis para agilizar a criação de novos contratos.</p>
                </div>
                <Button variant="outline" onClick={handleNewTemplate}>Criar Template</Button>
              </TactileCard>
            )}
          </TabsContent>

          <TabsContent value="contracts" className="mt-6">
            {contractsLoading ? (
              <div className="space-y-4">
                {[...Array(3)].map((_, i) => (
                  <Skeleton key={i} className="h-16 rounded-lg" />
                ))}
              </div>
            ) : contracts.length > 0 ? (
              <TactileCard className="p-0 overflow-hidden">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Título</TableHead>
                      <TableHead>Cliente</TableHead>
                      <TableHead>Status</TableHead>
                      <TableHead>Data de criação</TableHead>
                      <TableHead>Assinatura</TableHead>
                      <TableHead className="w-[120px]">Ações</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {contracts.map((contract: any) => (
                      <TableRow key={contract.id} data-testid={`row-contract-${contract.id}`}>
                        <TableCell className="font-medium">{contract.title}</TableCell>
                        <TableCell>{getClientName(contract.clientId)}</TableCell>
                        <TableCell>
                          <Badge className={statusColors[contract.status] || 'bg-gray-50 text-gray-700 border-gray-200'} variant="outline">
                            {statusMap[contract.status] || contract.status}
                          </Badge>
                        </TableCell>
                        <TableCell>
                          {format(new Date(contract.createdAt), "dd/MM/yyyy", { locale: ptBR })}
                        </TableCell>
                        <TableCell>
                          {contract.signedAt ? (
                            <div className="text-sm">
                              <p className="text-green-600 font-medium flex items-center gap-1">
                                <Check className="size-3" />
                                Assinado
                              </p>
                              <p className="text-muted-foreground text-xs">
                                {contract.signedByName} em {format(new Date(contract.signedAt), "dd/MM/yyyy 'às' HH:mm", { locale: ptBR })}
                              </p>
                            </div>
                          ) : (
                            <span className="text-muted-foreground text-sm">Pendente</span>
                          )}
                        </TableCell>
                        <TableCell>
                          <div className="flex items-center gap-1">
                            {contract.publicToken ? (
                              <Button
                                variant="ghost"
                                size="icon"
                                onClick={() => handleCopyLink(contract)}
                                title="Copiar link"
                              >
                                {copiedId === contract.id ? (
                                  <Check className="size-4 text-green-600" />
                                ) : (
                                  <Copy className="size-4" />
                                )}
                              </Button>
                            ) : (
                              <Button
                                variant="ghost"
                                size="icon"
                                onClick={() => generateLinkMutation.mutate(contract.id)}
                                title="Gerar link público"
                                disabled={generateLinkMutation.isPending}
                              >
                                <LinkIcon className="size-4" />
                              </Button>
                            )}
                            {contract.publicToken && (
                              <Button
                                variant="ghost"
                                size="icon"
                                onClick={() => window.open(`/c/${contract.publicToken}`, '_blank')}
                                title="Abrir link"
                              >
                                <ExternalLink className="size-4" />
                              </Button>
                            )}
                            <DropdownMenu>
                              <DropdownMenuTrigger asChild>
                                <Button variant="ghost" size="icon">
                                  <MoreHorizontal className="size-4" />
                                </Button>
                              </DropdownMenuTrigger>
                              <DropdownMenuContent align="end">
                                <DropdownMenuItem onClick={() => handleEditContract(contract)}>
                                  <Pencil className="size-4 mr-2" />
                                  Editar
                                </DropdownMenuItem>
                                <DropdownMenuItem onClick={() => handleDeleteContract(contract)} className="text-destructive">
                                  <Trash2 className="size-4 mr-2" />
                                  Excluir
                                </DropdownMenuItem>
                              </DropdownMenuContent>
                            </DropdownMenu>
                          </div>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TactileCard>
            ) : (
              <TactileCard className="min-h-[400px] flex flex-col items-center justify-center text-center p-8 space-y-6 border-dashed">
                <div className="size-20 rounded-full bg-primary/5 flex items-center justify-center">
                  <FileSignature className="size-10 text-primary/40" />
                </div>
                <div className="max-w-md space-y-2">
                  <h3 className="text-xl font-semibold">Nenhum contrato ainda</h3>
                  <p className="text-muted-foreground">Crie contratos e envie para assinatura digital. Gerencie todo o processo em um só lugar.</p>
                </div>
                <Button variant="outline" onClick={handleNewContract}>Criar Contrato</Button>
              </TactileCard>
            )}
          </TabsContent>
        </Tabs>
      </div>

      <TemplateModal
        open={templateModalOpen}
        onOpenChange={(open) => {
          setTemplateModalOpen(open);
          if (!open) setSelectedTemplate(null);
        }}
        template={selectedTemplate}
      />

      <ContractModal
        open={contractModalOpen}
        onOpenChange={(open) => {
          setContractModalOpen(open);
          if (!open) setSelectedContract(null);
        }}
        contract={selectedContract}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => {
          if (deleteType === 'template') {
            deleteTemplateMutation.mutate(selectedTemplate?.id);
          } else {
            deleteContractMutation.mutate(selectedContract?.id);
          }
        }}
        title={deleteType === 'template' ? "Excluir Template" : "Excluir Contrato"}
        description={deleteType === 'template' 
          ? `Tem certeza que deseja excluir o template "${selectedTemplate?.name}"?`
          : `Tem certeza que deseja excluir o contrato "${selectedContract?.title}"?`
        }
        isLoading={deleteTemplateMutation.isPending || deleteContractMutation.isPending}
      />
    </AppShell>
  );
}
