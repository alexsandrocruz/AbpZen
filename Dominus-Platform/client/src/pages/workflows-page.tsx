import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Switch } from "@/components/ui/switch";
import { Textarea } from "@/components/ui/textarea";
import { useToast } from "@/hooks/use-toast";
import { DeleteModal } from "@/components/modals/delete-modal";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Zap, Plus, MoreVertical, Play, Pause, Trash2, Settings, Activity } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

interface Workflow {
  id: string;
  name: string;
  description: string | null;
  status: 'ACTIVE' | 'PAUSED' | 'DRAFT';
  triggerType: string;
  triggerConditions: string | null;
  actions: string;
  executionCount: number;
  lastExecutedAt: string | null;
  createdAt: string;
}

const TRIGGER_TYPES = [
  { value: 'CLIENT_CREATED', label: 'Cliente criado' },
  { value: 'CLIENT_UPDATED', label: 'Cliente atualizado' },
  { value: 'PROJECT_CREATED', label: 'Projeto criado' },
  { value: 'PROJECT_STATUS_CHANGED', label: 'Status do projeto alterado' },
  { value: 'PROJECT_COMPLETED', label: 'Projeto concluído' },
  { value: 'TASK_CREATED', label: 'Tarefa criada' },
  { value: 'TASK_COMPLETED', label: 'Tarefa concluída' },
  { value: 'TASK_ASSIGNED', label: 'Tarefa atribuída' },
  { value: 'PROPOSAL_CREATED', label: 'Proposta criada' },
  { value: 'PROPOSAL_SENT', label: 'Proposta enviada' },
  { value: 'PROPOSAL_ACCEPTED', label: 'Proposta aceita' },
  { value: 'PROPOSAL_REJECTED', label: 'Proposta rejeitada' },
  { value: 'INVOICE_CREATED', label: 'Fatura criada' },
  { value: 'INVOICE_SENT', label: 'Fatura enviada' },
  { value: 'INVOICE_PAID', label: 'Fatura paga' },
  { value: 'INVOICE_OVERDUE', label: 'Fatura vencida' },
  { value: 'CONTRACT_CREATED', label: 'Contrato criado' },
  { value: 'CONTRACT_SIGNED', label: 'Contrato assinado' },
  { value: 'LEAD_CREATED', label: 'Lead criado' },
  { value: 'LEAD_STATUS_CHANGED', label: 'Status do lead alterado' },
  { value: 'DOCUMENT_EXPIRING', label: 'Documento expirando' },
];

const ACTION_TYPES = [
  { value: 'SEND_NOTIFICATION', label: 'Enviar notificação' },
  { value: 'CREATE_TASK', label: 'Criar tarefa' },
  { value: 'UPDATE_STATUS', label: 'Atualizar status' },
  { value: 'SEND_EMAIL', label: 'Enviar email' },
  { value: 'SEND_SMS', label: 'Enviar SMS' },
  { value: 'SEND_WHATSAPP', label: 'Enviar WhatsApp' },
  { value: 'CREATE_COMMENT', label: 'Adicionar comentário' },
  { value: 'ASSIGN_USER', label: 'Atribuir usuário' },
  { value: 'WAIT_DELAY', label: 'Aguardar' },
  { value: 'GENERATE_AI_CONTENT', label: 'Gerar conteúdo com IA' },
];

export default function WorkflowsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [showCreateDialog, setShowCreateDialog] = useState(false);
  const [editingWorkflow, setEditingWorkflow] = useState<Workflow | null>(null);
  const [deleteWorkflow, setDeleteWorkflow] = useState<Workflow | null>(null);

  const { data: emailTemplates = [] } = useQuery({
    queryKey: ['emailTemplates', currentWorkspace?.id],
    queryFn: () => api.getEmailTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: smsTemplates = [] } = useQuery({
    queryKey: ['smsTemplates', currentWorkspace?.id],
    queryFn: () => api.getSmsTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: whatsappTemplates = [] } = useQuery({
    queryKey: ['whatsappTemplates', currentWorkspace?.id],
    queryFn: () => api.getWhatsappTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const [formData, setFormData] = useState({
    name: '',
    description: '',
    triggerType: '',
    actions: [] as { type: string; config: Record<string, any>; order: number }[],
  });

  const { data: workflows = [], isLoading } = useQuery<Workflow[]>({
    queryKey: ["workflows", currentWorkspace?.id],
    queryFn: () => api.getWorkflows(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createMutation = useMutation({
    mutationFn: (data: typeof formData) => api.createWorkflow(currentWorkspace!.id, {
      ...data,
      status: 'DRAFT',
    }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
      toast({ title: "Sucesso", description: "Workflow criado com sucesso!" });
      setShowCreateDialog(false);
      resetForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => api.updateWorkflow(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
      toast({ title: "Sucesso", description: "Workflow atualizado!" });
      setEditingWorkflow(null);
      resetForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const toggleMutation = useMutation({
    mutationFn: (workflowId: string) => api.toggleWorkflow(currentWorkspace!.id, workflowId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
      toast({ title: "Sucesso", description: "Status do workflow alterado!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (workflowId: string) => api.deleteWorkflow(currentWorkspace!.id, workflowId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workflows"] });
      toast({ title: "Sucesso", description: "Workflow excluído!" });
      setDeleteWorkflow(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const testMutation = useMutation({
    mutationFn: (workflowId: string) => api.testWorkflow(currentWorkspace!.id, workflowId),
    onSuccess: (result) => {
      toast({ title: "Teste executado", description: `Status: ${result.status}` });
    },
    onError: (error: any) => {
      toast({ title: "Erro no teste", description: error.message, variant: "destructive" });
    },
  });

  const resetForm = () => {
    setFormData({
      name: '',
      description: '',
      triggerType: '',
      actions: [],
    });
  };

  const handleEdit = (workflow: Workflow) => {
    setEditingWorkflow(workflow);
    setFormData({
      name: workflow.name,
      description: workflow.description || '',
      triggerType: workflow.triggerType,
      actions: JSON.parse(workflow.actions || '[]'),
    });
  };

  const handleSubmit = () => {
    if (editingWorkflow) {
      updateMutation.mutate({ id: editingWorkflow.id, data: formData });
    } else {
      createMutation.mutate(formData);
    }
  };

  const addAction = () => {
    setFormData(prev => ({
      ...prev,
      actions: [...prev.actions, { type: 'SEND_NOTIFICATION', config: {}, order: prev.actions.length }],
    }));
  };

  const removeAction = (index: number) => {
    setFormData(prev => ({
      ...prev,
      actions: prev.actions.filter((_, i) => i !== index),
    }));
  };

  const updateAction = (index: number, field: string, value: any) => {
    setFormData(prev => ({
      ...prev,
      actions: prev.actions.map((action, i) => 
        i === index 
          ? field === 'type' 
            ? { ...action, type: value, config: {} }
            : { ...action, config: { ...action.config, [field]: value } }
          : action
      ),
    }));
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'ACTIVE':
        return <Badge className="bg-green-500/10 text-green-600 border-green-500/20">Ativo</Badge>;
      case 'PAUSED':
        return <Badge variant="secondary">Pausado</Badge>;
      case 'DRAFT':
        return <Badge variant="outline">Rascunho</Badge>;
      default:
        return <Badge variant="outline">{status}</Badge>;
    }
  };

  const getTriggerLabel = (type: string) => {
    return TRIGGER_TYPES.find(t => t.value === type)?.label || type;
  };

  if (!currentWorkspace) return null;

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight" data-testid="page-title-workflows">
              Workflows
            </h1>
            <p className="text-muted-foreground mt-1">
              Automatize tarefas repetitivas com gatilhos e ações
            </p>
          </div>
          <Button onClick={() => setShowCreateDialog(true)} data-testid="button-create-workflow">
            <Plus className="size-4 mr-2" />
            Novo Workflow
          </Button>
        </div>

        {isLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {[1, 2, 3].map(i => (
              <Skeleton key={i} className="h-48" />
            ))}
          </div>
        ) : workflows.length === 0 ? (
          <Card>
            <CardContent className="flex flex-col items-center justify-center py-12">
              <Zap className="size-12 text-muted-foreground mb-4" />
              <h3 className="text-lg font-medium">Nenhum workflow criado</h3>
              <p className="text-muted-foreground text-center max-w-md mt-2">
                Crie workflows para automatizar tarefas como enviar notificações, criar tarefas ou atualizar status automaticamente.
              </p>
              <Button className="mt-4" onClick={() => setShowCreateDialog(true)}>
                <Plus className="size-4 mr-2" />
                Criar primeiro workflow
              </Button>
            </CardContent>
          </Card>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {workflows.map(workflow => (
              <Card key={workflow.id} data-testid={`workflow-card-${workflow.id}`}>
                <CardHeader className="pb-2">
                  <div className="flex items-start justify-between">
                    <div className="flex-1">
                      <CardTitle className="text-lg flex items-center gap-2">
                        <Zap className={`size-4 ${workflow.status === 'ACTIVE' ? 'text-yellow-500' : 'text-muted-foreground'}`} />
                        {workflow.name}
                      </CardTitle>
                      <CardDescription className="mt-1">
                        {getTriggerLabel(workflow.triggerType)}
                      </CardDescription>
                    </div>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon">
                          <MoreVertical className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => handleEdit(workflow)}>
                          <Settings className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => testMutation.mutate(workflow.id)}>
                          <Play className="size-4 mr-2" />
                          Testar
                        </DropdownMenuItem>
                        <DropdownMenuItem 
                          onClick={() => setDeleteWorkflow(workflow)}
                          className="text-destructive"
                        >
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                </CardHeader>
                <CardContent>
                  {workflow.description && (
                    <p className="text-sm text-muted-foreground mb-3 line-clamp-2">
                      {workflow.description}
                    </p>
                  )}
                  
                  <div className="flex items-center justify-between mb-3">
                    {getStatusBadge(workflow.status)}
                    <div className="flex items-center gap-2">
                      <Switch
                        checked={workflow.status === 'ACTIVE'}
                        onCheckedChange={() => toggleMutation.mutate(workflow.id)}
                        data-testid={`toggle-workflow-${workflow.id}`}
                      />
                    </div>
                  </div>

                  <div className="flex items-center gap-4 text-xs text-muted-foreground">
                    <div className="flex items-center gap-1">
                      <Activity className="size-3" />
                      {workflow.executionCount} execuções
                    </div>
                    {workflow.lastExecutedAt && (
                      <div>
                        Última: {format(new Date(workflow.lastExecutedAt), "dd/MM HH:mm", { locale: ptBR })}
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        )}

        <Dialog open={showCreateDialog || !!editingWorkflow} onOpenChange={(open) => {
          if (!open) {
            setShowCreateDialog(false);
            setEditingWorkflow(null);
            resetForm();
          }
        }}>
          <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>
                {editingWorkflow ? 'Editar Workflow' : 'Novo Workflow'}
              </DialogTitle>
            </DialogHeader>

            <div className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="name">Nome *</Label>
                <Input
                  id="name"
                  value={formData.name}
                  onChange={e => setFormData(prev => ({ ...prev, name: e.target.value }))}
                  placeholder="Ex: Notificar equipe ao criar cliente"
                  data-testid="input-workflow-name"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="description">Descrição</Label>
                <Textarea
                  id="description"
                  value={formData.description}
                  onChange={e => setFormData(prev => ({ ...prev, description: e.target.value }))}
                  placeholder="Descreva o que este workflow faz..."
                  data-testid="input-workflow-description"
                />
              </div>

              <div className="space-y-2">
                <Label>Gatilho (Quando executar) *</Label>
                <Select
                  value={formData.triggerType}
                  onValueChange={value => setFormData(prev => ({ ...prev, triggerType: value }))}
                >
                  <SelectTrigger data-testid="select-trigger-type">
                    <SelectValue placeholder="Selecione o gatilho" />
                  </SelectTrigger>
                  <SelectContent>
                    {TRIGGER_TYPES.map(trigger => (
                      <SelectItem key={trigger.value} value={trigger.value}>
                        {trigger.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-3">
                <div className="flex items-center justify-between">
                  <Label>Ações (O que fazer)</Label>
                  <Button variant="outline" size="sm" onClick={addAction} data-testid="button-add-action">
                    <Plus className="size-3 mr-1" />
                    Adicionar ação
                  </Button>
                </div>

                {formData.actions.length === 0 ? (
                  <p className="text-sm text-muted-foreground text-center py-4 bg-muted/50 rounded-lg">
                    Adicione pelo menos uma ação para o workflow
                  </p>
                ) : (
                  <div className="space-y-3">
                    {formData.actions.map((action, index) => (
                      <Card key={index} className="p-3">
                        <div className="flex items-start gap-3">
                          <div className="flex-1 space-y-3">
                            <Select
                              value={action.type}
                              onValueChange={value => updateAction(index, 'type', value)}
                            >
                              <SelectTrigger>
                                <SelectValue />
                              </SelectTrigger>
                              <SelectContent>
                                {ACTION_TYPES.map(a => (
                                  <SelectItem key={a.value} value={a.value}>
                                    {a.label}
                                  </SelectItem>
                                ))}
                              </SelectContent>
                            </Select>

                            {action.type === 'SEND_NOTIFICATION' && (
                              <div className="space-y-2">
                                <Input
                                  placeholder="Título da notificação"
                                  value={action.config.title || ''}
                                  onChange={e => updateAction(index, 'title', e.target.value)}
                                />
                                <Textarea
                                  placeholder="Mensagem (use {{entity.nome}} para variáveis)"
                                  value={action.config.message || ''}
                                  onChange={e => updateAction(index, 'message', e.target.value)}
                                  rows={2}
                                />
                              </div>
                            )}

                            {action.type === 'CREATE_TASK' && (
                              <div className="space-y-2">
                                <Input
                                  placeholder="Título da tarefa"
                                  value={action.config.title || ''}
                                  onChange={e => updateAction(index, 'title', e.target.value)}
                                />
                                <Input
                                  placeholder="Prazo em dias (ex: 7)"
                                  type="number"
                                  value={action.config.dueDays || ''}
                                  onChange={e => updateAction(index, 'dueDays', parseInt(e.target.value))}
                                />
                              </div>
                            )}

                            {action.type === 'WAIT_DELAY' && (
                              <Input
                                placeholder="Minutos de espera"
                                type="number"
                                value={action.config.minutes || ''}
                                onChange={e => updateAction(index, 'minutes', parseInt(e.target.value))}
                              />
                            )}

                            {action.type === 'SEND_EMAIL' && (
                              <div className="space-y-2">
                                <Select
                                  value={action.config.templateId || ''}
                                  onValueChange={value => updateAction(index, 'templateId', value)}
                                >
                                  <SelectTrigger>
                                    <SelectValue placeholder="Selecione o template de email" />
                                  </SelectTrigger>
                                  <SelectContent>
                                    {emailTemplates.map((template: any) => (
                                      <SelectItem key={template.id} value={template.id}>
                                        {template.name}
                                      </SelectItem>
                                    ))}
                                  </SelectContent>
                                </Select>
                                <p className="text-xs text-muted-foreground">
                                  O email será enviado para o contato principal do cliente relacionado
                                </p>
                              </div>
                            )}

                            {action.type === 'SEND_SMS' && (
                              <div className="space-y-2">
                                <Select
                                  value={action.config.templateSlug || ''}
                                  onValueChange={value => updateAction(index, 'templateSlug', value)}
                                >
                                  <SelectTrigger>
                                    <SelectValue placeholder="Selecione o template de SMS" />
                                  </SelectTrigger>
                                  <SelectContent>
                                    {smsTemplates.map((template: any) => (
                                      <SelectItem key={template.id} value={template.slug}>
                                        {template.name}
                                      </SelectItem>
                                    ))}
                                  </SelectContent>
                                </Select>
                                <p className="text-xs text-muted-foreground">
                                  O SMS será enviado para o telefone do cliente relacionado
                                </p>
                              </div>
                            )}

                            {action.type === 'SEND_WHATSAPP' && (
                              <div className="space-y-2">
                                <Select
                                  value={action.config.templateSlug || ''}
                                  onValueChange={value => updateAction(index, 'templateSlug', value)}
                                >
                                  <SelectTrigger>
                                    <SelectValue placeholder="Selecione o template de WhatsApp" />
                                  </SelectTrigger>
                                  <SelectContent>
                                    {whatsappTemplates.map((template: any) => (
                                      <SelectItem key={template.id} value={template.slug}>
                                        {template.name}
                                      </SelectItem>
                                    ))}
                                  </SelectContent>
                                </Select>
                                <p className="text-xs text-muted-foreground">
                                  O WhatsApp será enviado para o telefone do cliente relacionado
                                </p>
                              </div>
                            )}
                          </div>
                          <Button
                            variant="ghost"
                            size="icon"
                            onClick={() => removeAction(index)}
                            className="text-destructive"
                          >
                            <Trash2 className="size-4" />
                          </Button>
                        </div>
                      </Card>
                    ))}
                  </div>
                )}
              </div>
            </div>

            <DialogFooter>
              <Button variant="outline" onClick={() => {
                setShowCreateDialog(false);
                setEditingWorkflow(null);
                resetForm();
              }}>
                Cancelar
              </Button>
              <Button 
                onClick={handleSubmit}
                disabled={!formData.name || !formData.triggerType || formData.actions.length === 0}
                data-testid="button-save-workflow"
              >
                {editingWorkflow ? 'Salvar' : 'Criar Workflow'}
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>

        <DeleteModal
          open={!!deleteWorkflow}
          onOpenChange={() => setDeleteWorkflow(null)}
          onConfirm={() => deleteWorkflow && deleteMutation.mutate(deleteWorkflow.id)}
          title="Excluir Workflow"
          description={`Tem certeza que deseja excluir o workflow "${deleteWorkflow?.name}"? Esta ação não pode ser desfeita.`}
        />
      </div>
    </AppShell>
  );
}
