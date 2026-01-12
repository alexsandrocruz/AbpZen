import { useState } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Skeleton } from "@/components/ui/skeleton";
import { Switch } from "@/components/ui/switch";
import { useToast } from "@/hooks/use-toast";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Plus, Settings2, MoreHorizontal, Pencil, Trash2, GripVertical, Users, FolderKanban, FileText, Receipt, UserPlus, CheckSquare, Palette, Mail, Copy, Eye, MessageSquare, MessageCircle, Phone, Send, Bot } from "lucide-react";
import { DeleteModal } from "@/components/modals/delete-modal";

const ENTITY_TYPES = [
  { value: 'CLIENT', label: 'Clientes', icon: Users },
  { value: 'PROJECT', label: 'Projetos', icon: FolderKanban },
  { value: 'PROPOSAL', label: 'Propostas', icon: FileText },
  { value: 'INVOICE', label: 'Faturas', icon: Receipt },
  { value: 'LEAD', label: 'Leads', icon: UserPlus },
  { value: 'TASK', label: 'Tarefas', icon: CheckSquare },
];

const FIELD_TYPES = [
  { value: 'TEXT', label: 'Texto' },
  { value: 'TEXTAREA', label: 'Texto Longo' },
  { value: 'NUMBER', label: 'Número' },
  { value: 'CURRENCY', label: 'Moeda (R$)' },
  { value: 'DATE', label: 'Data' },
  { value: 'CHECKBOX', label: 'Checkbox (Sim/Não)' },
  { value: 'SELECT', label: 'Seleção Única' },
  { value: 'MULTISELECT', label: 'Seleção Múltipla' },
  { value: 'URL', label: 'Link/URL' },
  { value: 'EMAIL', label: 'E-mail' },
  { value: 'PHONE', label: 'Telefone' },
];

export default function SettingsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [selectedEntityType, setSelectedEntityType] = useState('CLIENT');
  const [modalOpen, setModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [selectedField, setSelectedField] = useState<any>(null);

  const [formData, setFormData] = useState({
    fieldKey: '',
    label: '',
    fieldType: 'TEXT',
    isRequired: false,
    placeholder: '',
    helpText: '',
    defaultValue: '',
    options: '',
  });

  const [emailModalOpen, setEmailModalOpen] = useState(false);
  const [deleteEmailModalOpen, setDeleteEmailModalOpen] = useState(false);
  const [selectedEmailTemplate, setSelectedEmailTemplate] = useState<any>(null);
  const [previewOpen, setPreviewOpen] = useState(false);
  const [emailFormData, setEmailFormData] = useState({
    name: '',
    slug: '',
    subject: '',
    htmlContent: '',
    textContent: '',
    category: 'TRANSACTIONAL',
  });

  const [smsModalOpen, setSmsModalOpen] = useState(false);
  const [deleteSmsModalOpen, setDeleteSmsModalOpen] = useState(false);
  const [selectedSmsTemplate, setSelectedSmsTemplate] = useState<any>(null);
  const [smsFormData, setSmsFormData] = useState({
    name: '',
    slug: '',
    content: '',
    category: 'TRANSACTIONAL',
  });

  const [whatsappModalOpen, setWhatsappModalOpen] = useState(false);
  const [deleteWhatsappModalOpen, setDeleteWhatsappModalOpen] = useState(false);
  const [selectedWhatsappTemplate, setSelectedWhatsappTemplate] = useState<any>(null);
  const [whatsappFormData, setWhatsappFormData] = useState({
    name: '',
    slug: '',
    content: '',
    category: 'TRANSACTIONAL',
  });

  const { data: customFields = [], isLoading } = useQuery({
    queryKey: ['customFields', currentWorkspace?.id, selectedEntityType],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, selectedEntityType),
    enabled: !!currentWorkspace,
  });

  const { data: proposalTemplates = [] } = useQuery({
    queryKey: ['proposalTemplates', currentWorkspace?.id],
    queryFn: () => api.getProposalTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: emailTemplates = [], isLoading: isLoadingEmailTemplates } = useQuery({
    queryKey: ['emailTemplates', currentWorkspace?.id],
    queryFn: () => api.getEmailTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: smsTemplates = [], isLoading: isLoadingSmsTemplates } = useQuery({
    queryKey: ['smsTemplates', currentWorkspace?.id],
    queryFn: () => api.getSmsTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: smsSettings } = useQuery({
    queryKey: ['smsSettings', currentWorkspace?.id],
    queryFn: () => api.getSmsSettings(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: smsUsage } = useQuery({
    queryKey: ['smsUsage', currentWorkspace?.id],
    queryFn: () => api.getSmsUsage(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: whatsappTemplates = [], isLoading: isLoadingWhatsappTemplates } = useQuery({
    queryKey: ['whatsappTemplates', currentWorkspace?.id],
    queryFn: () => api.getWhatsappTemplates(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: whatsappSettings } = useQuery({
    queryKey: ['whatsappSettings', currentWorkspace?.id],
    queryFn: () => api.getWhatsappSettings(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: whatsappUsage } = useQuery({
    queryKey: ['whatsappUsage', currentWorkspace?.id],
    queryFn: () => api.getWhatsappUsage(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createEmailTemplateMutation = useMutation({
    mutationFn: (data: any) => api.createEmailTemplate(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['emailTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template de email criado!" });
      handleCloseEmailModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateEmailTemplateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => 
      api.updateEmailTemplate(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['emailTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template atualizado!" });
      handleCloseEmailModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteEmailTemplateMutation = useMutation({
    mutationFn: (id: string) => api.deleteEmailTemplate(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['emailTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template excluído!" });
      setDeleteEmailModalOpen(false);
      setSelectedEmailTemplate(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const createSmsTemplateMutation = useMutation({
    mutationFn: (data: any) => api.createSmsTemplate(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['smsTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template de SMS criado!" });
      handleCloseSmsModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateSmsTemplateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => 
      api.updateSmsTemplate(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['smsTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template atualizado!" });
      handleCloseSmsModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteSmsTemplateMutation = useMutation({
    mutationFn: (id: string) => api.deleteSmsTemplate(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['smsTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template excluído!" });
      setDeleteSmsModalOpen(false);
      setSelectedSmsTemplate(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateSmsSettingsMutation = useMutation({
    mutationFn: (data: any) => api.updateSmsSettings(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['smsSettings', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Configurações de SMS atualizadas!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const createWhatsappTemplateMutation = useMutation({
    mutationFn: (data: any) => api.createWhatsappTemplate(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['whatsappTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template de WhatsApp criado!" });
      handleCloseWhatsappModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateWhatsappTemplateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => 
      api.updateWhatsappTemplate(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['whatsappTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template atualizado!" });
      handleCloseWhatsappModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteWhatsappTemplateMutation = useMutation({
    mutationFn: (id: string) => api.deleteWhatsappTemplate(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['whatsappTemplates', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Template excluído!" });
      setDeleteWhatsappModalOpen(false);
      setSelectedWhatsappTemplate(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateWhatsappSettingsMutation = useMutation({
    mutationFn: (data: any) => api.updateWhatsappSettings(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['whatsappSettings', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Configurações de WhatsApp atualizadas!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateWorkspaceMutation = useMutation({
    mutationFn: (data: { defaultProposalTemplateId: string | null }) => 
      api.updateWorkspace(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['workspaces'] });
      toast({ title: "Sucesso", description: "Template padrão atualizado!" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createCustomField(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customFields', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Campo personalizado criado!" });
      handleCloseModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => 
      api.updateCustomField(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customFields', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Campo atualizado!" });
      handleCloseModal();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (fieldId: string) => api.deleteCustomField(currentWorkspace!.id, fieldId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['customFields', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Campo excluído!" });
      setDeleteModalOpen(false);
      setSelectedField(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleOpenModal = (field?: any) => {
    if (field) {
      setSelectedField(field);
      setFormData({
        fieldKey: field.fieldKey || '',
        label: field.label || '',
        fieldType: field.fieldType || 'TEXT',
        isRequired: field.isRequired || false,
        placeholder: field.placeholder || '',
        helpText: field.helpText || '',
        defaultValue: field.defaultValue || '',
        options: field.options || '',
      });
    } else {
      setSelectedField(null);
      setFormData({
        fieldKey: '',
        label: '',
        fieldType: 'TEXT',
        isRequired: false,
        placeholder: '',
        helpText: '',
        defaultValue: '',
        options: '',
      });
    }
    setModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
    setSelectedField(null);
  };

  const handleSubmit = () => {
    if (!formData.label || !formData.fieldKey) {
      toast({ title: "Erro", description: "Nome e chave do campo são obrigatórios", variant: "destructive" });
      return;
    }

    const data = {
      ...formData,
      entityType: selectedEntityType,
      options: formData.options || null,
    };

    if (selectedField) {
      updateMutation.mutate({ id: selectedField.id, data });
    } else {
      createMutation.mutate(data);
    }
  };

  const handleDeleteField = (field: any) => {
    setSelectedField(field);
    setDeleteModalOpen(true);
  };

  const generateFieldKey = (label: string) => {
    return label
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9]+/g, '_')
      .replace(/^_|_$/g, '');
  };

  const showOptionsField = formData.fieldType === 'SELECT' || formData.fieldType === 'MULTISELECT';

  const getFieldTypeLabel = (type: string) => {
    return FIELD_TYPES.find(f => f.value === type)?.label || type;
  };

  const handleOpenEmailModal = (template?: any) => {
    if (template) {
      setSelectedEmailTemplate(template);
      setEmailFormData({
        name: template.name || '',
        slug: template.slug || '',
        subject: template.subject || '',
        htmlContent: template.htmlContent || '',
        textContent: template.textContent || '',
        category: template.category || 'TRANSACTIONAL',
      });
    } else {
      setSelectedEmailTemplate(null);
      setEmailFormData({
        name: '',
        slug: '',
        subject: '',
        htmlContent: '',
        textContent: '',
        category: 'TRANSACTIONAL',
      });
    }
    setEmailModalOpen(true);
  };

  const handleCloseEmailModal = () => {
    setEmailModalOpen(false);
    setSelectedEmailTemplate(null);
  };

  const handleEmailSubmit = () => {
    if (!emailFormData.name || !emailFormData.slug || !emailFormData.subject) {
      toast({ title: "Erro", description: "Nome, identificador e assunto são obrigatórios", variant: "destructive" });
      return;
    }

    if (selectedEmailTemplate) {
      updateEmailTemplateMutation.mutate({ id: selectedEmailTemplate.id, data: emailFormData });
    } else {
      createEmailTemplateMutation.mutate(emailFormData);
    }
  };

  const handleDeleteEmailTemplate = (template: any) => {
    setSelectedEmailTemplate(template);
    setDeleteEmailModalOpen(true);
  };

  const handleOpenSmsModal = (template?: any) => {
    if (template) {
      setSelectedSmsTemplate(template);
      setSmsFormData({
        name: template.name || '',
        slug: template.slug || '',
        content: template.content || '',
        category: template.category || 'TRANSACTIONAL',
      });
    } else {
      setSelectedSmsTemplate(null);
      setSmsFormData({
        name: '',
        slug: '',
        content: '',
        category: 'TRANSACTIONAL',
      });
    }
    setSmsModalOpen(true);
  };

  const handleCloseSmsModal = () => {
    setSmsModalOpen(false);
    setSelectedSmsTemplate(null);
  };

  const handleSmsSubmit = () => {
    if (!smsFormData.name || !smsFormData.slug || !smsFormData.content) {
      toast({ title: "Erro", description: "Nome, identificador e conteúdo são obrigatórios", variant: "destructive" });
      return;
    }

    if (selectedSmsTemplate) {
      updateSmsTemplateMutation.mutate({ id: selectedSmsTemplate.id, data: smsFormData });
    } else {
      createSmsTemplateMutation.mutate(smsFormData);
    }
  };

  const handleDeleteSmsTemplate = (template: any) => {
    setSelectedSmsTemplate(template);
    setDeleteSmsModalOpen(true);
  };

  const handleOpenWhatsappModal = (template?: any) => {
    if (template) {
      setSelectedWhatsappTemplate(template);
      setWhatsappFormData({
        name: template.name || '',
        slug: template.slug || '',
        content: template.content || '',
        category: template.category || 'TRANSACTIONAL',
      });
    } else {
      setSelectedWhatsappTemplate(null);
      setWhatsappFormData({
        name: '',
        slug: '',
        content: '',
        category: 'TRANSACTIONAL',
      });
    }
    setWhatsappModalOpen(true);
  };

  const handleCloseWhatsappModal = () => {
    setWhatsappModalOpen(false);
    setSelectedWhatsappTemplate(null);
  };

  const handleWhatsappSubmit = () => {
    if (!whatsappFormData.name || !whatsappFormData.slug || !whatsappFormData.content) {
      toast({ title: "Erro", description: "Nome, identificador e conteúdo são obrigatórios", variant: "destructive" });
      return;
    }

    if (selectedWhatsappTemplate) {
      updateWhatsappTemplateMutation.mutate({ id: selectedWhatsappTemplate.id, data: whatsappFormData });
    } else {
      createWhatsappTemplateMutation.mutate(whatsappFormData);
    }
  };

  const handleDeleteWhatsappTemplate = (template: any) => {
    setSelectedWhatsappTemplate(template);
    setDeleteWhatsappModalOpen(true);
  };

  const generateSlug = (name: string) => {
    return name
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9]+/g, '_')
      .replace(/^_|_$/g, '');
  };

  const getCategoryLabel = (category: string) => {
    const categories: Record<string, string> = {
      'TRANSACTIONAL': 'Transacional',
      'MARKETING': 'Marketing',
      'NOTIFICATION': 'Notificação',
      'SYSTEM': 'Sistema',
    };
    return categories[category] || category;
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Configurações</h1>
            <p className="text-muted-foreground">Gerencie campos personalizados do seu workspace.</p>
          </div>
        </div>

        <Tabs defaultValue="custom-fields">
          <TabsList>
            <TabsTrigger value="custom-fields" className="gap-2">
              <Settings2 className="size-4" />
              Campos Personalizados
            </TabsTrigger>
            <TabsTrigger value="proposals" className="gap-2">
              <Palette className="size-4" />
              Propostas
            </TabsTrigger>
            <TabsTrigger value="email-templates" className="gap-2">
              <Mail className="size-4" />
              Modelos de Email
            </TabsTrigger>
            <TabsTrigger value="sms-templates" className="gap-2">
              <MessageSquare className="size-4" />
              SMS
            </TabsTrigger>
            <TabsTrigger value="whatsapp-templates" className="gap-2">
              <MessageCircle className="size-4" />
              WhatsApp
            </TabsTrigger>
          </TabsList>

          <TabsContent value="custom-fields" className="mt-6 space-y-6">
            <TactileCard className="p-6">
              <div className="flex items-center justify-between mb-6">
                <div>
                  <h2 className="text-xl font-semibold">Campos Personalizados</h2>
                  <p className="text-sm text-muted-foreground">
                    Adicione campos extras para suas entidades
                  </p>
                </div>
                <Button className="gap-2" onClick={() => handleOpenModal()} data-testid="button-add-custom-field">
                  <Plus className="size-4" />
                  Novo Campo
                </Button>
              </div>

              <div className="flex gap-2 mb-6 flex-wrap">
                {ENTITY_TYPES.map((entity) => {
                  const Icon = entity.icon;
                  return (
                    <Button
                      key={entity.value}
                      variant={selectedEntityType === entity.value ? "default" : "outline"}
                      size="sm"
                      className="gap-2"
                      onClick={() => setSelectedEntityType(entity.value)}
                      data-testid={`button-entity-${entity.value.toLowerCase()}`}
                    >
                      <Icon className="size-4" />
                      {entity.label}
                    </Button>
                  );
                })}
              </div>

              {isLoading ? (
                <div className="space-y-3">
                  {[...Array(3)].map((_, i) => (
                    <Skeleton key={i} className="h-16 rounded-lg" />
                  ))}
                </div>
              ) : customFields.length > 0 ? (
                <div className="space-y-3">
                  {customFields.map((field: any, index: number) => (
                    <div
                      key={field.id}
                      className="flex items-center gap-4 p-4 border rounded-lg bg-card hover:bg-muted/50 transition-colors"
                      data-testid={`custom-field-${field.id}`}
                    >
                      <GripVertical className="size-5 text-muted-foreground cursor-grab" />
                      <div className="flex-1">
                        <div className="flex items-center gap-2">
                          <span className="font-medium">{field.label}</span>
                          {field.isRequired && (
                            <span className="text-xs text-red-500">*</span>
                          )}
                        </div>
                        <div className="flex items-center gap-2 text-sm text-muted-foreground">
                          <code className="text-xs bg-muted px-1.5 py-0.5 rounded">{field.fieldKey}</code>
                          <span>•</span>
                          <span>{getFieldTypeLabel(field.fieldType)}</span>
                        </div>
                      </div>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => handleOpenModal(field)}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem 
                            onClick={() => handleDeleteField(field)}
                            className="text-destructive"
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="text-center py-12 text-muted-foreground">
                  <Settings2 className="size-12 mx-auto mb-4 opacity-50" />
                  <p>Nenhum campo personalizado para {ENTITY_TYPES.find(e => e.value === selectedEntityType)?.label}</p>
                  <p className="text-sm">Clique em "Novo Campo" para adicionar</p>
                </div>
              )}
            </TactileCard>
          </TabsContent>

          <TabsContent value="proposals" className="mt-6 space-y-6">
            <TactileCard className="p-6">
              <div className="mb-6">
                <h2 className="text-xl font-semibold">Template Padrão de Propostas</h2>
                <p className="text-sm text-muted-foreground">
                  Selecione o template que será usado como padrão ao criar novas propostas
                </p>
              </div>

              <div className="space-y-4">
                <div className="space-y-2">
                  <Label>Template Padrão</Label>
                  <Select
                    value={(currentWorkspace as any)?.defaultProposalTemplateId || "none"}
                    onValueChange={(value) => {
                      updateWorkspaceMutation.mutate({ 
                        defaultProposalTemplateId: value === "none" ? null : value 
                      });
                    }}
                  >
                    <SelectTrigger data-testid="select-default-template">
                      <SelectValue placeholder="Selecione um template" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="none">Nenhum (proposta em branco)</SelectItem>
                      {proposalTemplates.map((template: any) => (
                        <SelectItem key={template.id} value={template.id}>
                          {template.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <p className="text-xs text-muted-foreground">
                    O template selecionado será aplicado automaticamente ao criar novas propostas visuais
                  </p>
                </div>

                {proposalTemplates.length > 0 && (
                  <div className="pt-4 border-t">
                    <h3 className="font-medium mb-3">Templates Disponíveis ({proposalTemplates.length})</h3>
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                      {proposalTemplates.map((template: any) => {
                        const style = template.style ? JSON.parse(template.style) : {};
                        return (
                          <div
                            key={template.id}
                            className="border rounded-lg p-4 hover:border-primary transition-colors cursor-pointer"
                            style={{ borderLeftColor: style.primaryColor, borderLeftWidth: '4px' }}
                            onClick={() => {
                              updateWorkspaceMutation.mutate({ defaultProposalTemplateId: template.id });
                            }}
                            data-testid={`template-card-${template.id}`}
                          >
                            <div className="flex items-center gap-2 mb-2">
                              <div 
                                className="size-3 rounded-full"
                                style={{ backgroundColor: style.primaryColor }}
                              />
                              <span className="font-medium text-sm">{template.name}</span>
                            </div>
                            {template.description && (
                              <p className="text-xs text-muted-foreground line-clamp-2">
                                {template.description}
                              </p>
                            )}
                            {(currentWorkspace as any)?.defaultProposalTemplateId === template.id && (
                              <span className="mt-2 inline-flex items-center px-2 py-1 text-xs font-medium bg-primary/10 text-primary rounded">
                                Padrão
                              </span>
                            )}
                          </div>
                        );
                      })}
                    </div>
                  </div>
                )}
              </div>
            </TactileCard>
          </TabsContent>

          <TabsContent value="email-templates" className="mt-6 space-y-6">
            <TactileCard className="p-6">
              <div className="flex items-center justify-between mb-6">
                <div>
                  <h2 className="text-xl font-semibold">Modelos de Email</h2>
                  <p className="text-sm text-muted-foreground">
                    Crie e gerencie modelos de email para comunicação automatizada
                  </p>
                </div>
                <Button className="gap-2" onClick={() => handleOpenEmailModal()} data-testid="button-add-email-template">
                  <Plus className="size-4" />
                  Novo Modelo
                </Button>
              </div>

              {isLoadingEmailTemplates ? (
                <div className="space-y-3">
                  {[...Array(3)].map((_, i) => (
                    <Skeleton key={i} className="h-20 rounded-lg" />
                  ))}
                </div>
              ) : emailTemplates.length > 0 ? (
                <div className="space-y-3">
                  {emailTemplates.map((template: any) => (
                    <div
                      key={template.id}
                      className="flex items-center gap-4 p-4 border rounded-lg bg-card hover:bg-muted/50 transition-colors"
                      data-testid={`email-template-${template.id}`}
                    >
                      <div className="size-10 rounded-lg bg-primary/10 flex items-center justify-center">
                        <Mail className="size-5 text-primary" />
                      </div>
                      <div className="flex-1 min-w-0">
                        <div className="flex items-center gap-2">
                          <span className="font-medium">{template.name}</span>
                          {template.isSystem && (
                            <span className="text-xs bg-blue-100 text-blue-700 px-2 py-0.5 rounded">Sistema</span>
                          )}
                        </div>
                        <div className="flex items-center gap-2 text-sm text-muted-foreground">
                          <code className="text-xs bg-muted px-1.5 py-0.5 rounded">{template.slug}</code>
                          <span>•</span>
                          <span>{getCategoryLabel(template.category)}</span>
                        </div>
                        <p className="text-sm text-muted-foreground truncate mt-1">
                          Assunto: {template.subject}
                        </p>
                      </div>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => {
                            setSelectedEmailTemplate(template);
                            setPreviewOpen(true);
                          }}>
                            <Eye className="size-4 mr-2" />
                            Visualizar
                          </DropdownMenuItem>
                          <DropdownMenuItem onClick={() => handleOpenEmailModal(template)} disabled={template.isSystem}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem 
                            onClick={() => handleDeleteEmailTemplate(template)}
                            className="text-destructive"
                            disabled={template.isSystem}
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="text-center py-12 text-muted-foreground">
                  <Mail className="size-12 mx-auto mb-4 opacity-50" />
                  <p>Nenhum modelo de email cadastrado</p>
                  <p className="text-sm">Clique em "Novo Modelo" para criar</p>
                </div>
              )}

              <div className="mt-6 p-4 bg-muted/50 rounded-lg">
                <h3 className="font-medium mb-2">Variáveis Disponíveis</h3>
                <p className="text-sm text-muted-foreground mb-3">
                  Use estas variáveis no conteúdo do email. Elas serão substituídas pelos valores reais ao enviar.
                </p>
                <div className="flex flex-wrap gap-2">
                  {['{{nome}}', '{{email}}', '{{workspace}}', '{{link}}', '{{data}}'].map((variable) => (
                    <code 
                      key={variable}
                      className="text-xs bg-background border px-2 py-1 rounded cursor-pointer hover:bg-muted"
                      onClick={() => {
                        navigator.clipboard.writeText(variable);
                        toast({ title: "Copiado!", description: `${variable} copiado para a área de transferência` });
                      }}
                    >
                      {variable}
                    </code>
                  ))}
                </div>
              </div>
            </TactileCard>
          </TabsContent>

          <TabsContent value="sms-templates" className="mt-6 space-y-6">
            <TactileCard className="p-6">
              <div className="mb-6">
                <h2 className="text-xl font-semibold">Configurações de SMS</h2>
                <p className="text-sm text-muted-foreground">
                  Ative as notificações por SMS para seus clientes
                </p>
              </div>

              {smsUsage && (
                <div className="mb-6 p-4 border rounded-lg bg-muted/30">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="font-medium">Uso de SMS</h3>
                      <p className="text-sm text-muted-foreground">
                        {smsUsage.quota === 0 
                          ? 'Uso ilimitado' 
                          : `${smsUsage.used} de ${smsUsage.quota} mensagens utilizadas`}
                      </p>
                    </div>
                    {smsUsage.quota > 0 && (
                      <div className="text-right">
                        <span className="text-2xl font-bold">
                          {smsUsage.remaining}
                        </span>
                        <p className="text-xs text-muted-foreground">restantes</p>
                      </div>
                    )}
                  </div>
                  {smsUsage.quota > 0 && (
                    <div className="mt-3">
                      <div className="w-full bg-muted rounded-full h-2">
                        <div 
                          className={`h-2 rounded-full ${smsUsage.percentUsed > 80 ? 'bg-red-500' : smsUsage.percentUsed > 50 ? 'bg-yellow-500' : 'bg-green-500'}`}
                          style={{ width: `${Math.min(smsUsage.percentUsed, 100)}%` }}
                        />
                      </div>
                      <p className="text-xs text-muted-foreground mt-1">
                        Renova em: {smsUsage.resetsAt ? new Date(smsUsage.resetsAt).toLocaleDateString('pt-BR') : 'sem data definida'}
                      </p>
                    </div>
                  )}
                  {smsUsage.hasCustomCredentials && (
                    <p className="text-xs text-green-600 mt-2 flex items-center gap-1">
                      <span className="size-2 bg-green-500 rounded-full"></span>
                      Usando credenciais Twilio próprias
                    </p>
                  )}
                </div>
              )}

              <div className="space-y-4 border-b pb-6 mb-6">
                <div className="flex items-center justify-between">
                  <div>
                    <Label>Ativar SMS</Label>
                    <p className="text-sm text-muted-foreground">
                      Enviar notificações por SMS para clientes
                    </p>
                  </div>
                  <Switch
                    checked={smsSettings?.smsEnabled || false}
                    onCheckedChange={(checked) => {
                      updateSmsSettingsMutation.mutate({ smsEnabled: checked });
                    }}
                    data-testid="switch-sms-enabled"
                  />
                </div>

                <div className="flex items-center justify-between">
                  <div>
                    <Label>Lembretes de Cobrança Automáticos</Label>
                    <p className="text-sm text-muted-foreground">
                      Enviar SMS automático para faturas vencidas
                    </p>
                  </div>
                  <Switch
                    checked={smsSettings?.autoInvoiceReminder || false}
                    onCheckedChange={(checked) => {
                      updateSmsSettingsMutation.mutate({ autoInvoiceReminder: checked });
                    }}
                    data-testid="switch-auto-reminder"
                  />
                </div>

                {smsSettings?.autoInvoiceReminder && (
                  <div className="grid grid-cols-2 gap-4 pt-4">
                    <div className="space-y-2">
                      <Label>Dias antes do vencimento</Label>
                      <Input
                        type="number"
                        min="1"
                        max="30"
                        value={smsSettings?.reminderDaysBefore || 3}
                        onChange={(e) => {
                          updateSmsSettingsMutation.mutate({ reminderDaysBefore: parseInt(e.target.value) || 3 });
                        }}
                        data-testid="input-days-before"
                      />
                      <p className="text-xs text-muted-foreground">Lembrete antes do vencimento</p>
                    </div>
                    <div className="space-y-2">
                      <Label>Dias após o vencimento</Label>
                      <Input
                        type="number"
                        min="1"
                        max="30"
                        value={smsSettings?.reminderDaysAfter || 1}
                        onChange={(e) => {
                          updateSmsSettingsMutation.mutate({ reminderDaysAfter: parseInt(e.target.value) || 1 });
                        }}
                        data-testid="input-days-after"
                      />
                      <p className="text-xs text-muted-foreground">Cobrança após vencimento</p>
                    </div>
                  </div>
                )}
              </div>

              <div className="flex items-center justify-between mb-6">
                <div>
                  <h2 className="text-xl font-semibold">Modelos de SMS</h2>
                  <p className="text-sm text-muted-foreground">
                    Gerencie os templates de SMS para diferentes eventos
                  </p>
                </div>
                <Button className="gap-2" onClick={() => handleOpenSmsModal()} data-testid="button-add-sms-template">
                  <Plus className="size-4" />
                  Novo Modelo
                </Button>
              </div>

              {isLoadingSmsTemplates ? (
                <div className="space-y-3">
                  {[...Array(3)].map((_, i) => (
                    <Skeleton key={i} className="h-16 rounded-lg" />
                  ))}
                </div>
              ) : smsTemplates.length > 0 ? (
                <div className="space-y-3">
                  {smsTemplates.map((template: any) => (
                    <div
                      key={template.id}
                      className="flex items-center gap-4 p-4 border rounded-lg bg-card hover:bg-muted/50 transition-colors"
                      data-testid={`sms-template-${template.id}`}
                    >
                      <div className="size-10 rounded-lg bg-green-500/10 flex items-center justify-center">
                        <MessageSquare className="size-5 text-green-600" />
                      </div>
                      <div className="flex-1 min-w-0">
                        <div className="flex items-center gap-2">
                          <span className="font-medium">{template.name}</span>
                          {template.isSystem && (
                            <span className="text-xs bg-blue-100 text-blue-700 px-2 py-0.5 rounded">Sistema</span>
                          )}
                        </div>
                        <div className="flex items-center gap-2 text-sm text-muted-foreground">
                          <code className="text-xs bg-muted px-1.5 py-0.5 rounded">{template.slug}</code>
                          <span>•</span>
                          <span>{getCategoryLabel(template.category)}</span>
                        </div>
                        <p className="text-sm text-muted-foreground truncate mt-1">
                          {template.content}
                        </p>
                      </div>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => handleOpenSmsModal(template)} disabled={template.isSystem}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem 
                            onClick={() => handleDeleteSmsTemplate(template)}
                            className="text-destructive"
                            disabled={template.isSystem}
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="text-center py-12 text-muted-foreground">
                  <MessageSquare className="size-12 mx-auto mb-4 opacity-50" />
                  <p>Nenhum modelo de SMS cadastrado</p>
                  <p className="text-sm">Clique em "Novo Modelo" para criar</p>
                </div>
              )}

              <div className="mt-6 p-4 bg-muted/50 rounded-lg">
                <h3 className="font-medium mb-2">Variáveis Disponíveis</h3>
                <p className="text-sm text-muted-foreground mb-3">
                  Use estas variáveis no conteúdo do SMS. Elas serão substituídas pelos valores reais ao enviar.
                </p>
                <div className="flex flex-wrap gap-2">
                  {['{{nome}}', '{{proposta}}', '{{contrato}}', '{{fatura}}', '{{valor}}', '{{vencimento}}', '{{link}}'].map((variable) => (
                    <code 
                      key={variable}
                      className="text-xs bg-background border px-2 py-1 rounded cursor-pointer hover:bg-muted"
                      onClick={() => {
                        navigator.clipboard.writeText(variable);
                        toast({ title: "Copiado!", description: `${variable} copiado para a área de transferência` });
                      }}
                    >
                      {variable}
                    </code>
                  ))}
                </div>
              </div>
            </TactileCard>
          </TabsContent>

          <TabsContent value="whatsapp-templates" className="mt-6 space-y-6">
            <TactileCard className="p-6">
              <div className="mb-6">
                <h2 className="text-xl font-semibold">Configurações de WhatsApp</h2>
                <p className="text-sm text-muted-foreground">
                  Ative as notificações por WhatsApp e o chatbot automático
                </p>
              </div>

              {whatsappUsage && (
                <div className="mb-6 p-4 border rounded-lg bg-muted/30">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="font-medium">Uso de WhatsApp</h3>
                      <p className="text-sm text-muted-foreground">
                        {whatsappUsage.quota === 0 
                          ? 'Uso ilimitado' 
                          : `${whatsappUsage.used} de ${whatsappUsage.quota} mensagens utilizadas`}
                      </p>
                    </div>
                    {whatsappUsage.quota > 0 && (
                      <div className="text-right">
                        <span className="text-2xl font-bold">
                          {whatsappUsage.remaining}
                        </span>
                        <p className="text-xs text-muted-foreground">restantes</p>
                      </div>
                    )}
                  </div>
                  {whatsappUsage.quota > 0 && (
                    <div className="mt-3">
                      <div className="w-full bg-muted rounded-full h-2">
                        <div 
                          className={`h-2 rounded-full ${whatsappUsage.percentUsed > 80 ? 'bg-red-500' : whatsappUsage.percentUsed > 50 ? 'bg-yellow-500' : 'bg-green-500'}`}
                          style={{ width: `${Math.min(whatsappUsage.percentUsed, 100)}%` }}
                        />
                      </div>
                      <p className="text-xs text-muted-foreground mt-1">
                        Renova em: {whatsappUsage.resetsAt ? new Date(whatsappUsage.resetsAt).toLocaleDateString('pt-BR') : 'sem data definida'}
                      </p>
                    </div>
                  )}
                  {whatsappUsage.hasCustomCredentials && (
                    <p className="text-xs text-green-600 mt-2 flex items-center gap-1">
                      <span className="size-2 bg-green-500 rounded-full"></span>
                      Usando credenciais Twilio próprias
                    </p>
                  )}
                </div>
              )}

              <div className="space-y-4 border-b pb-6 mb-6">
                <div className="flex items-center justify-between">
                  <div>
                    <Label>Ativar WhatsApp</Label>
                    <p className="text-sm text-muted-foreground">
                      Enviar notificações por WhatsApp para clientes
                    </p>
                  </div>
                  <Switch
                    checked={whatsappSettings?.whatsappEnabled || false}
                    onCheckedChange={(checked) => {
                      updateWhatsappSettingsMutation.mutate({ whatsappEnabled: checked });
                    }}
                    data-testid="switch-whatsapp-enabled"
                  />
                </div>

                <div className="flex items-center justify-between">
                  <div>
                    <Label>Lembretes de Cobrança via WhatsApp</Label>
                    <p className="text-sm text-muted-foreground">
                      Enviar WhatsApp automático para faturas vencidas
                    </p>
                  </div>
                  <Switch
                    checked={whatsappSettings?.autoInvoiceReminder || false}
                    onCheckedChange={(checked) => {
                      updateWhatsappSettingsMutation.mutate({ autoInvoiceReminder: checked });
                    }}
                    data-testid="switch-whatsapp-auto-reminder"
                  />
                </div>

                {whatsappSettings?.autoInvoiceReminder && (
                  <div className="grid grid-cols-2 gap-4 pt-4">
                    <div className="space-y-2">
                      <Label>Dias antes do vencimento</Label>
                      <Input
                        type="number"
                        min="1"
                        max="30"
                        value={whatsappSettings?.reminderDaysBefore || 3}
                        onChange={(e) => {
                          updateWhatsappSettingsMutation.mutate({ reminderDaysBefore: parseInt(e.target.value) || 3 });
                        }}
                        data-testid="input-whatsapp-days-before"
                      />
                      <p className="text-xs text-muted-foreground">Lembrete antes do vencimento</p>
                    </div>
                    <div className="space-y-2">
                      <Label>Dias após o vencimento</Label>
                      <Input
                        type="number"
                        min="1"
                        max="30"
                        value={whatsappSettings?.reminderDaysAfter || 1}
                        onChange={(e) => {
                          updateWhatsappSettingsMutation.mutate({ reminderDaysAfter: parseInt(e.target.value) || 1 });
                        }}
                        data-testid="input-whatsapp-days-after"
                      />
                      <p className="text-xs text-muted-foreground">Cobrança após vencimento</p>
                    </div>
                  </div>
                )}
              </div>

              <div className="space-y-4 border-b pb-6 mb-6">
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-3">
                    <div className="size-10 rounded-lg bg-purple-500/10 flex items-center justify-center">
                      <Bot className="size-5 text-purple-600" />
                    </div>
                    <div>
                      <Label>Chatbot com IA</Label>
                      <p className="text-sm text-muted-foreground">
                        Responda automaticamente usando ChatGPT com contexto do seu negócio
                      </p>
                    </div>
                  </div>
                  <Switch
                    checked={whatsappSettings?.chatbotEnabled || false}
                    onCheckedChange={(checked) => {
                      updateWhatsappSettingsMutation.mutate({ chatbotEnabled: checked });
                    }}
                    data-testid="switch-chatbot-enabled"
                  />
                </div>

                {whatsappSettings?.chatbotEnabled && (
                  <div className="mt-4 p-4 bg-purple-50 dark:bg-purple-950/20 rounded-lg border border-purple-200 dark:border-purple-800">
                    <h4 className="font-medium text-purple-700 dark:text-purple-300 mb-2">Funcionalidades do Chatbot</h4>
                    <ul className="text-sm text-purple-600 dark:text-purple-400 space-y-1">
                      <li>• Responde perguntas sobre seus serviços automaticamente</li>
                      <li>• Verifica disponibilidade na agenda e pode agendar horários</li>
                      <li>• Informa status de propostas, contratos e faturas</li>
                      <li>• Reagenda compromissos quando solicitado</li>
                      <li>• Responde dúvidas gerais sobre seu negócio</li>
                    </ul>
                  </div>
                )}
              </div>

              <div className="flex items-center justify-between mb-6">
                <div>
                  <h2 className="text-xl font-semibold">Modelos de WhatsApp</h2>
                  <p className="text-sm text-muted-foreground">
                    Gerencie os templates de WhatsApp para diferentes eventos
                  </p>
                </div>
                <Button className="gap-2" onClick={() => handleOpenWhatsappModal()} data-testid="button-add-whatsapp-template">
                  <Plus className="size-4" />
                  Novo Modelo
                </Button>
              </div>

              {isLoadingWhatsappTemplates ? (
                <div className="space-y-3">
                  {[...Array(3)].map((_, i) => (
                    <Skeleton key={i} className="h-16 rounded-lg" />
                  ))}
                </div>
              ) : whatsappTemplates.length > 0 ? (
                <div className="space-y-3">
                  {whatsappTemplates.map((template: any) => (
                    <div
                      key={template.id}
                      className="flex items-center gap-4 p-4 border rounded-lg bg-card hover:bg-muted/50 transition-colors"
                      data-testid={`whatsapp-template-${template.id}`}
                    >
                      <div className="size-10 rounded-lg bg-emerald-500/10 flex items-center justify-center">
                        <MessageCircle className="size-5 text-emerald-600" />
                      </div>
                      <div className="flex-1 min-w-0">
                        <div className="flex items-center gap-2">
                          <span className="font-medium">{template.name}</span>
                          {template.isSystem && (
                            <span className="text-xs bg-blue-100 text-blue-700 px-2 py-0.5 rounded">Sistema</span>
                          )}
                        </div>
                        <div className="flex items-center gap-2 text-sm text-muted-foreground">
                          <code className="text-xs bg-muted px-1.5 py-0.5 rounded">{template.slug}</code>
                          <span>•</span>
                          <span>{getCategoryLabel(template.category)}</span>
                        </div>
                        <p className="text-sm text-muted-foreground truncate mt-1">
                          {template.content}
                        </p>
                      </div>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon">
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => handleOpenWhatsappModal(template)} disabled={template.isSystem}>
                            <Pencil className="size-4 mr-2" />
                            Editar
                          </DropdownMenuItem>
                          <DropdownMenuItem 
                            onClick={() => handleDeleteWhatsappTemplate(template)}
                            className="text-destructive"
                            disabled={template.isSystem}
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </div>
                  ))}
                </div>
              ) : (
                <div className="text-center py-12 text-muted-foreground">
                  <MessageCircle className="size-12 mx-auto mb-4 opacity-50" />
                  <p>Nenhum modelo de WhatsApp cadastrado</p>
                  <p className="text-sm">Clique em "Novo Modelo" para criar</p>
                </div>
              )}

              <div className="mt-6 p-4 bg-muted/50 rounded-lg">
                <h3 className="font-medium mb-2">Variáveis Disponíveis</h3>
                <p className="text-sm text-muted-foreground mb-3">
                  Use estas variáveis no conteúdo do WhatsApp. Elas serão substituídas pelos valores reais ao enviar.
                </p>
                <div className="flex flex-wrap gap-2">
                  {['{{nome}}', '{{proposta}}', '{{contrato}}', '{{fatura}}', '{{valor}}', '{{vencimento}}', '{{link}}'].map((variable) => (
                    <code 
                      key={variable}
                      className="text-xs bg-background border px-2 py-1 rounded cursor-pointer hover:bg-muted"
                      onClick={() => {
                        navigator.clipboard.writeText(variable);
                        toast({ title: "Copiado!", description: `${variable} copiado para a área de transferência` });
                      }}
                    >
                      {variable}
                    </code>
                  ))}
                </div>
              </div>
            </TactileCard>
          </TabsContent>
        </Tabs>
      </div>

      <Dialog open={modalOpen} onOpenChange={setModalOpen}>
        <DialogContent className="max-w-md">
          <DialogHeader>
            <DialogTitle>
              {selectedField ? 'Editar Campo' : 'Novo Campo Personalizado'}
            </DialogTitle>
          </DialogHeader>

          <div className="space-y-4 mt-4">
            <div className="space-y-2">
              <Label>Nome do Campo *</Label>
              <Input
                placeholder="Ex: CNAE Principal"
                value={formData.label}
                onChange={(e) => {
                  const label = e.target.value;
                  setFormData({ 
                    ...formData, 
                    label,
                    fieldKey: formData.fieldKey || generateFieldKey(label)
                  });
                }}
                data-testid="input-field-label"
              />
            </div>

            <div className="space-y-2">
              <Label>Chave do Campo *</Label>
              <Input
                placeholder="Ex: cnae_principal"
                value={formData.fieldKey}
                onChange={(e) => setFormData({ ...formData, fieldKey: e.target.value })}
                data-testid="input-field-key"
              />
              <p className="text-xs text-muted-foreground">Identificador único, sem espaços</p>
            </div>

            <div className="space-y-2">
              <Label>Tipo do Campo</Label>
              <Select
                value={formData.fieldType}
                onValueChange={(value) => setFormData({ ...formData, fieldType: value })}
              >
                <SelectTrigger data-testid="select-field-type">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {FIELD_TYPES.map((type) => (
                    <SelectItem key={type.value} value={type.value}>
                      {type.label}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            {showOptionsField && (
              <div className="space-y-2">
                <Label>Opções (uma por linha)</Label>
                <Textarea
                  placeholder="Opção 1&#10;Opção 2&#10;Opção 3"
                  value={formData.options}
                  onChange={(e) => setFormData({ ...formData, options: e.target.value })}
                  rows={4}
                  data-testid="input-field-options"
                />
              </div>
            )}

            <div className="space-y-2">
              <Label>Placeholder</Label>
              <Input
                placeholder="Texto de exemplo"
                value={formData.placeholder}
                onChange={(e) => setFormData({ ...formData, placeholder: e.target.value })}
                data-testid="input-field-placeholder"
              />
            </div>

            <div className="space-y-2">
              <Label>Texto de Ajuda</Label>
              <Input
                placeholder="Instruções para o usuário"
                value={formData.helpText}
                onChange={(e) => setFormData({ ...formData, helpText: e.target.value })}
                data-testid="input-field-help"
              />
            </div>

            <div className="space-y-2">
              <Label>Valor Padrão</Label>
              <Input
                placeholder="Valor inicial do campo"
                value={formData.defaultValue}
                onChange={(e) => setFormData({ ...formData, defaultValue: e.target.value })}
                data-testid="input-field-default"
              />
            </div>

            <div className="flex items-center justify-between">
              <Label htmlFor="required-switch">Campo Obrigatório</Label>
              <Switch
                id="required-switch"
                checked={formData.isRequired}
                onCheckedChange={(checked) => setFormData({ ...formData, isRequired: checked })}
                data-testid="switch-field-required"
              />
            </div>

            <div className="flex gap-2 pt-4">
              <Button variant="outline" className="flex-1" onClick={handleCloseModal}>
                Cancelar
              </Button>
              <Button 
                className="flex-1" 
                onClick={handleSubmit}
                disabled={createMutation.isPending || updateMutation.isPending}
                data-testid="button-save-field"
              >
                {selectedField ? 'Salvar' : 'Criar Campo'}
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        onConfirm={() => deleteMutation.mutate(selectedField?.id)}
        title="Excluir Campo"
        description={`Tem certeza que deseja excluir o campo "${selectedField?.label}"? Todos os valores salvos serão perdidos.`}
        isLoading={deleteMutation.isPending}
      />

      <Dialog open={emailModalOpen} onOpenChange={setEmailModalOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>
              {selectedEmailTemplate ? 'Editar Modelo de Email' : 'Novo Modelo de Email'}
            </DialogTitle>
          </DialogHeader>

          <div className="space-y-4 mt-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Nome do Modelo *</Label>
                <Input
                  placeholder="Ex: Boas-vindas ao Cliente"
                  value={emailFormData.name}
                  onChange={(e) => {
                    const name = e.target.value;
                    setEmailFormData({ 
                      ...emailFormData, 
                      name,
                      slug: emailFormData.slug || generateSlug(name)
                    });
                  }}
                  data-testid="input-email-name"
                />
              </div>

              <div className="space-y-2">
                <Label>Identificador (slug) *</Label>
                <Input
                  placeholder="Ex: boas_vindas_cliente"
                  value={emailFormData.slug}
                  onChange={(e) => setEmailFormData({ ...emailFormData, slug: e.target.value })}
                  data-testid="input-email-slug"
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Assunto *</Label>
                <Input
                  placeholder="Ex: Bem-vindo à {{workspace}}!"
                  value={emailFormData.subject}
                  onChange={(e) => setEmailFormData({ ...emailFormData, subject: e.target.value })}
                  data-testid="input-email-subject"
                />
              </div>

              <div className="space-y-2">
                <Label>Categoria</Label>
                <Select
                  value={emailFormData.category}
                  onValueChange={(value) => setEmailFormData({ ...emailFormData, category: value })}
                >
                  <SelectTrigger data-testid="select-email-category">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="TRANSACTIONAL">Transacional</SelectItem>
                    <SelectItem value="MARKETING">Marketing</SelectItem>
                    <SelectItem value="NOTIFICATION">Notificação</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>

            <div className="space-y-2">
              <Label>Conteúdo HTML</Label>
              <Textarea
                placeholder="<h1>Olá {{nome}}</h1><p>Bem-vindo ao {{workspace}}!</p>"
                value={emailFormData.htmlContent}
                onChange={(e) => setEmailFormData({ ...emailFormData, htmlContent: e.target.value })}
                rows={8}
                className="font-mono text-sm"
                data-testid="input-email-html"
              />
              <p className="text-xs text-muted-foreground">
                Use HTML para formatar o email. Variáveis: {'{{'} nome {'}}'}, {'{{'} email {'}}'}, {'{{'} workspace {'}}'}, etc.
              </p>
            </div>

            <div className="space-y-2">
              <Label>Conteúdo Texto (fallback)</Label>
              <Textarea
                placeholder="Olá {{nome}}, bem-vindo ao {{workspace}}!"
                value={emailFormData.textContent}
                onChange={(e) => setEmailFormData({ ...emailFormData, textContent: e.target.value })}
                rows={4}
                data-testid="input-email-text"
              />
              <p className="text-xs text-muted-foreground">
                Versão em texto puro para clientes de email que não suportam HTML.
              </p>
            </div>

            <div className="flex gap-2 pt-4">
              <Button variant="outline" className="flex-1" onClick={handleCloseEmailModal}>
                Cancelar
              </Button>
              <Button 
                className="flex-1" 
                onClick={handleEmailSubmit}
                disabled={createEmailTemplateMutation.isPending || updateEmailTemplateMutation.isPending}
                data-testid="button-save-email-template"
              >
                {selectedEmailTemplate ? 'Salvar' : 'Criar Modelo'}
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      <Dialog open={previewOpen} onOpenChange={setPreviewOpen}>
        <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>Pré-visualização: {selectedEmailTemplate?.name}</DialogTitle>
          </DialogHeader>

          <div className="space-y-4 mt-4">
            <div className="p-3 bg-muted rounded-lg">
              <p className="text-sm"><strong>Assunto:</strong> {selectedEmailTemplate?.subject}</p>
            </div>
            
            <div className="border rounded-lg overflow-hidden">
              <div className="bg-muted px-3 py-2 text-sm font-medium border-b">
                Conteúdo HTML
              </div>
              <iframe 
                className="w-full min-h-[300px] bg-white"
                sandbox="allow-same-origin"
                srcDoc={selectedEmailTemplate?.htmlContent || '<p style="color: #666; padding: 16px;">Sem conteúdo HTML</p>'}
                title="Preview do email"
              />
            </div>

            {selectedEmailTemplate?.textContent && (
              <div className="border rounded-lg overflow-hidden">
                <div className="bg-muted px-3 py-2 text-sm font-medium border-b">
                  Conteúdo Texto
                </div>
                <pre className="p-4 text-sm whitespace-pre-wrap">{selectedEmailTemplate?.textContent}</pre>
              </div>
            )}
          </div>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteEmailModalOpen}
        onOpenChange={setDeleteEmailModalOpen}
        onConfirm={() => deleteEmailTemplateMutation.mutate(selectedEmailTemplate?.id)}
        title="Excluir Modelo de Email"
        description={`Tem certeza que deseja excluir o modelo "${selectedEmailTemplate?.name}"?`}
        isLoading={deleteEmailTemplateMutation.isPending}
      />

      <Dialog open={smsModalOpen} onOpenChange={setSmsModalOpen}>
        <DialogContent className="max-w-lg">
          <DialogHeader>
            <DialogTitle>
              {selectedSmsTemplate ? 'Editar Modelo de SMS' : 'Novo Modelo de SMS'}
            </DialogTitle>
          </DialogHeader>

          <div className="space-y-4 mt-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Nome do Modelo *</Label>
                <Input
                  placeholder="Ex: Lembrete de Pagamento"
                  value={smsFormData.name}
                  onChange={(e) => {
                    const name = e.target.value;
                    setSmsFormData({ 
                      ...smsFormData, 
                      name,
                      slug: smsFormData.slug || generateSlug(name)
                    });
                  }}
                  data-testid="input-sms-name"
                />
              </div>

              <div className="space-y-2">
                <Label>Identificador (slug) *</Label>
                <Input
                  placeholder="Ex: lembrete_pagamento"
                  value={smsFormData.slug}
                  onChange={(e) => setSmsFormData({ ...smsFormData, slug: e.target.value })}
                  data-testid="input-sms-slug"
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label>Categoria</Label>
              <Select
                value={smsFormData.category}
                onValueChange={(value) => setSmsFormData({ ...smsFormData, category: value })}
              >
                <SelectTrigger data-testid="select-sms-category">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="TRANSACTIONAL">Transacional</SelectItem>
                  <SelectItem value="MARKETING">Marketing</SelectItem>
                  <SelectItem value="NOTIFICATION">Notificação</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label>Conteúdo da Mensagem *</Label>
              <Textarea
                placeholder="Olá {{nome}}, sua fatura de R$ {{valor}} vence em {{vencimento}}. Acesse: {{link}}"
                value={smsFormData.content}
                onChange={(e) => setSmsFormData({ ...smsFormData, content: e.target.value })}
                rows={4}
                data-testid="input-sms-content"
              />
              <p className="text-xs text-muted-foreground">
                Max. 160 caracteres para SMS simples. Variáveis: {'{{nome}}'}, {'{{valor}}'}, {'{{link}}'}, etc.
              </p>
              <p className="text-xs text-muted-foreground">
                Caracteres: {smsFormData.content.length} / 160
              </p>
            </div>

            <div className="flex gap-2 pt-4">
              <Button variant="outline" className="flex-1" onClick={handleCloseSmsModal}>
                Cancelar
              </Button>
              <Button 
                className="flex-1" 
                onClick={handleSmsSubmit}
                disabled={createSmsTemplateMutation.isPending || updateSmsTemplateMutation.isPending}
                data-testid="button-save-sms-template"
              >
                {selectedSmsTemplate ? 'Salvar' : 'Criar Modelo'}
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteSmsModalOpen}
        onOpenChange={setDeleteSmsModalOpen}
        onConfirm={() => deleteSmsTemplateMutation.mutate(selectedSmsTemplate?.id)}
        title="Excluir Modelo de SMS"
        description={`Tem certeza que deseja excluir o modelo "${selectedSmsTemplate?.name}"?`}
        isLoading={deleteSmsTemplateMutation.isPending}
      />

      <Dialog open={whatsappModalOpen} onOpenChange={setWhatsappModalOpen}>
        <DialogContent className="max-w-lg">
          <DialogHeader>
            <DialogTitle>
              {selectedWhatsappTemplate ? 'Editar Modelo de WhatsApp' : 'Novo Modelo de WhatsApp'}
            </DialogTitle>
          </DialogHeader>

          <div className="space-y-4 mt-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Nome do Modelo *</Label>
                <Input
                  placeholder="Ex: Lembrete de Pagamento"
                  value={whatsappFormData.name}
                  onChange={(e) => {
                    const name = e.target.value;
                    setWhatsappFormData({ 
                      ...whatsappFormData, 
                      name,
                      slug: whatsappFormData.slug || generateSlug(name)
                    });
                  }}
                  data-testid="input-whatsapp-name"
                />
              </div>

              <div className="space-y-2">
                <Label>Identificador (slug) *</Label>
                <Input
                  placeholder="Ex: lembrete_pagamento"
                  value={whatsappFormData.slug}
                  onChange={(e) => setWhatsappFormData({ ...whatsappFormData, slug: e.target.value })}
                  data-testid="input-whatsapp-slug"
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label>Categoria</Label>
              <Select
                value={whatsappFormData.category}
                onValueChange={(value) => setWhatsappFormData({ ...whatsappFormData, category: value })}
              >
                <SelectTrigger data-testid="select-whatsapp-category">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="TRANSACTIONAL">Transacional</SelectItem>
                  <SelectItem value="MARKETING">Marketing</SelectItem>
                  <SelectItem value="NOTIFICATION">Notificação</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label>Conteúdo da Mensagem *</Label>
              <Textarea
                placeholder="Olá {{nome}}, sua fatura de R$ {{valor}} vence em {{vencimento}}. Acesse: {{link}}"
                value={whatsappFormData.content}
                onChange={(e) => setWhatsappFormData({ ...whatsappFormData, content: e.target.value })}
                rows={4}
                data-testid="input-whatsapp-content"
              />
              <p className="text-xs text-muted-foreground">
                WhatsApp suporta mensagens maiores. Variáveis: {'{{nome}}'}, {'{{valor}}'}, {'{{link}}'}, etc.
              </p>
            </div>

            <div className="flex gap-2 pt-4">
              <Button variant="outline" className="flex-1" onClick={handleCloseWhatsappModal}>
                Cancelar
              </Button>
              <Button 
                className="flex-1" 
                onClick={handleWhatsappSubmit}
                disabled={createWhatsappTemplateMutation.isPending || updateWhatsappTemplateMutation.isPending}
                data-testid="button-save-whatsapp-template"
              >
                {selectedWhatsappTemplate ? 'Salvar' : 'Criar Modelo'}
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      <DeleteModal
        open={deleteWhatsappModalOpen}
        onOpenChange={setDeleteWhatsappModalOpen}
        onConfirm={() => deleteWhatsappTemplateMutation.mutate(selectedWhatsappTemplate?.id)}
        title="Excluir Modelo de WhatsApp"
        description={`Tem certeza que deseja excluir o modelo "${selectedWhatsappTemplate?.name}"?`}
        isLoading={deleteWhatsappTemplateMutation.isPending}
      />
    </AppShell>
  );
}
