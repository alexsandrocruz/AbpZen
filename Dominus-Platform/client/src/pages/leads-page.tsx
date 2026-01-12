import { useState, useRef, useEffect } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";
import { Plus, MoreHorizontal, Pencil, Trash2, Download, Upload, Users, Phone, Mail, Search, X, Loader2, Calendar, Link2, Link2Off, Filter, ChevronLeft, ChevronRight, Tag, Sparkles, AlertTriangle, Copy, ArrowUpDown } from "lucide-react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { DeleteModal } from "@/components/modals/delete-modal";
import { useToast } from "@/hooks/use-toast";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
  DropdownMenuSeparator,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Checkbox } from "@/components/ui/checkbox";

const statusLabels: Record<string, string> = {
  NEW: 'Novo',
  CONTACTED: 'Contatado',
  QUALIFIED: 'Qualificado',
  CONVERTED: 'Convertido',
  LOST: 'Perdido',
};

const statusColors: Record<string, string> = {
  NEW: 'bg-blue-100 text-blue-800',
  CONTACTED: 'bg-yellow-100 text-yellow-800',
  QUALIFIED: 'bg-green-100 text-green-800',
  CONVERTED: 'bg-emerald-100 text-emerald-800',
  LOST: 'bg-red-100 text-red-800',
};

const sourceLabels: Record<string, string> = {
  DIRECT: 'Direto',
  REFERRAL: 'Indicação',
  CAMPAIGN: 'Campanha',
  ORGANIC: 'Orgânico',
  PAID: 'Pago',
  IMPORT: 'Importação',
  BOOKING: 'Agendamento',
  GOOGLE_CONTACTS: 'Google Contacts',
  FORM: 'Formulário',
};

interface LeadTag {
  id: string;
  name: string;
  color: string | null;
}

interface CustomFieldDefinition {
  id: string;
  fieldKey: string;
  label: string;
  fieldType: 'TEXT' | 'NUMBER' | 'DATE' | 'CHECKBOX' | 'SELECT';
  selectOptions: string[] | null;
  isRequired: boolean;
}

interface CustomFieldValue {
  definitionId: string;
  valueText?: string | null;
  valueNumber?: string | null;
  valueDate?: string | null;
  valueBoolean?: boolean | null;
}

interface LeadModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  lead?: any;
  onSuccess: () => void;
}

function LeadModal({ open, onOpenChange, lead, onSuccess }: LeadModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phone: '',
    source: 'DIRECT',
    status: 'NEW',
    notes: '',
  });
  const [selectedTagIds, setSelectedTagIds] = useState<string[]>([]);
  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});
  const [tagSearch, setTagSearch] = useState('');
  const [isCreatingTag, setIsCreatingTag] = useState(false);

  const { data: availableTags = [] } = useQuery({
    queryKey: ['lead-tags', currentWorkspace?.id],
    queryFn: () => api.getLeadTags(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: customFields = [] } = useQuery<CustomFieldDefinition[]>({
    queryKey: ['custom-fields', currentWorkspace?.id, 'LEAD'],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, 'LEAD'),
    enabled: !!currentWorkspace && open,
  });

  const { data: existingCustomValues = [] } = useQuery<CustomFieldValue[]>({
    queryKey: ['custom-field-values', currentWorkspace?.id, 'LEAD', lead?.id],
    queryFn: () => api.getCustomFieldValues(currentWorkspace!.id, 'LEAD', lead!.id),
    enabled: !!currentWorkspace && !!lead?.id && open,
  });

  useEffect(() => {
    if (lead) {
      setFormData({
        name: lead.name || '',
        email: lead.email || '',
        phone: lead.phone || '',
        source: lead.source || 'DIRECT',
        status: lead.status || 'NEW',
        notes: lead.notes || '',
      });
      setSelectedTagIds(lead.tags?.map((t: LeadTag) => t.id) || []);
      setCustomFieldValues({});
      setTagSearch('');
    } else {
      setFormData({
        name: '',
        email: '',
        phone: '',
        source: 'DIRECT',
        status: 'NEW',
        notes: '',
      });
      setSelectedTagIds([]);
      setCustomFieldValues({});
      setTagSearch('');
    }
  }, [lead, open]);

  useEffect(() => {
    if (existingCustomValues.length === 0) return;
    const values: Record<string, any> = {};
    existingCustomValues.forEach((cv) => {
      if (cv.valueText !== null && cv.valueText !== undefined) {
        values[cv.definitionId] = cv.valueText;
      } else if (cv.valueNumber !== null && cv.valueNumber !== undefined) {
        values[cv.definitionId] = cv.valueNumber;
      } else if (cv.valueDate !== null && cv.valueDate !== undefined) {
        values[cv.definitionId] = cv.valueDate;
      } else if (cv.valueBoolean !== null && cv.valueBoolean !== undefined) {
        values[cv.definitionId] = cv.valueBoolean;
      }
    });
    setCustomFieldValues(values);
  }, [existingCustomValues.length]);

  const saveCustomFields = async (leadId: string) => {
    if (customFields.length === 0) return;
    const values = customFields.map((field) => {
      const value = customFieldValues[field.id];
      const isEmpty = value === undefined || value === '' || value === null;
      return {
        definitionId: field.id,
        valueText: field.fieldType === 'TEXT' || field.fieldType === 'SELECT' ? (isEmpty ? null : value) : null,
        valueNumber: field.fieldType === 'NUMBER' ? (isEmpty ? null : value) : null,
        valueDate: field.fieldType === 'DATE' ? (isEmpty ? null : value) : null,
        valueBoolean: field.fieldType === 'CHECKBOX' ? (value ?? false) : null,
      };
    });
    await api.saveCustomFieldValues(currentWorkspace!.id, 'LEAD', leadId, values);
  };

  const createMutation = useMutation({
    mutationFn: async (data: any) => {
      const newLead = await api.createLead(currentWorkspace!.id, data);
      if (selectedTagIds.length > 0) {
        await api.setLeadTags(currentWorkspace!.id, newLead.id, selectedTagIds);
      }
      await saveCustomFields(newLead.id);
      return newLead;
    },
    onSuccess: () => {
      toast({ title: "Sucesso", description: "Lead criado com sucesso!" });
      queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
      onSuccess();
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: async ({ data, leadId }: { data: any; leadId: string }) => {
      const errors: string[] = [];
      const updatedLead = await api.updateLead(currentWorkspace!.id, leadId, data);
      try {
        await api.setLeadTags(currentWorkspace!.id, leadId, selectedTagIds);
      } catch (tagError: any) {
        console.error('Error setting tags:', tagError);
        errors.push('tags');
      }
      try {
        await saveCustomFields(leadId);
      } catch (cfError: any) {
        console.error('Error saving custom fields:', cfError);
        errors.push('campos personalizados');
      }
      return { updatedLead, errors };
    },
    onSuccess: ({ errors }) => {
      if (errors.length > 0) {
        toast({ 
          title: "Lead atualizado com avisos", 
          description: `Não foi possível salvar: ${errors.join(', ')}`,
          variant: "destructive"
        });
      } else {
        toast({ title: "Sucesso", description: "Lead atualizado com sucesso!" });
      }
      queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
      onSuccess();
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message || "Erro ao atualizar lead", variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (lead?.id) {
      updateMutation.mutate({ data: formData, leadId: lead.id });
    } else {
      createMutation.mutate(formData);
    }
  };

  const toggleTag = (tagId: string) => {
    setSelectedTagIds(prev => 
      prev.includes(tagId) 
        ? prev.filter(id => id !== tagId)
        : [...prev, tagId]
    );
  };

  const createTagMutation = useMutation({
    mutationFn: (name: string) => api.createLeadTag(currentWorkspace!.id, { name, color: '#6b7280' }),
    onSuccess: (newTag) => {
      queryClient.invalidateQueries({ queryKey: ['lead-tags'] });
      setSelectedTagIds(prev => [...prev, newTag.id]);
      setTagSearch('');
      setIsCreatingTag(false);
      toast({ title: "Sucesso", description: `Tag "${newTag.name}" criada!` });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
      setIsCreatingTag(false);
    },
  });

  const handleCreateTag = () => {
    if (tagSearch.trim()) {
      setIsCreatingTag(true);
      createTagMutation.mutate(tagSearch.trim());
    }
  };

  const filteredTags = availableTags.filter((tag: LeadTag) =>
    tag.name.toLowerCase().includes(tagSearch.toLowerCase())
  );

  const showCreateOption = tagSearch.trim() && 
    !availableTags.some((tag: LeadTag) => tag.name.toLowerCase() === tagSearch.toLowerCase().trim());

  const isLoading = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-md max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{lead ? 'Editar Lead' : 'Novo Lead'}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome</Label>
            <Input
              id="name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="Nome do lead"
              data-testid="input-lead-name"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              value={formData.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              placeholder="email@exemplo.com"
              data-testid="input-lead-email"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="phone">Telefone</Label>
            <Input
              id="phone"
              value={formData.phone}
              onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
              placeholder="(00) 00000-0000"
              data-testid="input-lead-phone"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="status">Status</Label>
            <Select
              value={formData.status}
              onValueChange={(value) => setFormData({ ...formData, status: value })}
            >
              <SelectTrigger data-testid="select-lead-status">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="NEW">Novo</SelectItem>
                <SelectItem value="CONTACTED">Contatado</SelectItem>
                <SelectItem value="QUALIFIED">Qualificado</SelectItem>
                <SelectItem value="CONVERTED">Convertido</SelectItem>
                <SelectItem value="LOST">Perdido</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="space-y-2">
            <Label htmlFor="source">Origem</Label>
            <Select
              value={formData.source}
              onValueChange={(value) => setFormData({ ...formData, source: value })}
            >
              <SelectTrigger data-testid="select-lead-source">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="DIRECT">Direto</SelectItem>
                <SelectItem value="REFERRAL">Indicação</SelectItem>
                <SelectItem value="CAMPAIGN">Campanha</SelectItem>
                <SelectItem value="ORGANIC">Orgânico</SelectItem>
                <SelectItem value="PAID">Pago</SelectItem>
                <SelectItem value="IMPORT">Importação</SelectItem>
                <SelectItem value="BOOKING">Agendamento</SelectItem>
                <SelectItem value="GOOGLE_CONTACTS">Google Contacts</SelectItem>
                <SelectItem value="FORM">Formulário</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="space-y-2">
            <Label>Tags</Label>
            {selectedTagIds.length > 0 && (
              <div className="flex flex-wrap gap-1.5 mb-2">
                {selectedTagIds.map((tagId) => {
                  const tag = availableTags.find((t: LeadTag) => t.id === tagId);
                  if (!tag) return null;
                  return (
                    <Badge
                      key={tagId}
                      variant="secondary"
                      className="flex items-center gap-1 pr-1"
                      style={{ backgroundColor: `${tag.color || '#6b7280'}20`, borderColor: tag.color || '#6b7280' }}
                    >
                      <span 
                        className="w-2 h-2 rounded-full" 
                        style={{ backgroundColor: tag.color || '#6b7280' }}
                      />
                      {tag.name}
                      <button
                        type="button"
                        onClick={() => toggleTag(tagId)}
                        className="ml-1 hover:bg-black/10 rounded p-0.5"
                        data-testid={`remove-tag-${tagId}`}
                      >
                        <X className="size-3" />
                      </button>
                    </Badge>
                  );
                })}
              </div>
            )}
            <div className="relative">
              <Input
                placeholder="Buscar ou criar tag..."
                value={tagSearch}
                onChange={(e) => setTagSearch(e.target.value)}
                data-testid="input-tag-search"
              />
            </div>
            <div className="border rounded-md max-h-[120px] overflow-y-auto">
              {showCreateOption && (
                <button
                  type="button"
                  onClick={handleCreateTag}
                  disabled={isCreatingTag}
                  className="w-full px-3 py-2 text-left text-sm hover:bg-muted flex items-center gap-2 text-primary font-medium"
                  data-testid="button-create-tag"
                >
                  {isCreatingTag ? (
                    <Loader2 className="size-4 animate-spin" />
                  ) : (
                    <Plus className="size-4" />
                  )}
                  Criar tag "{tagSearch.trim()}"
                </button>
              )}
              {filteredTags.length === 0 && !showCreateOption ? (
                <div className="px-3 py-2 text-sm text-muted-foreground">
                  {tagSearch ? 'Nenhuma tag encontrada' : 'Digite para buscar ou criar tags'}
                </div>
              ) : (
                filteredTags.map((tag: LeadTag) => (
                  <button
                    type="button"
                    key={tag.id}
                    onClick={(e) => {
                      e.preventDefault();
                      e.stopPropagation();
                      toggleTag(tag.id);
                    }}
                    className={`w-full px-3 py-2 text-left text-sm hover:bg-muted flex items-center gap-2 ${
                      selectedTagIds.includes(tag.id) ? 'bg-muted' : ''
                    }`}
                    data-testid={`select-tag-${tag.id}`}
                  >
                    <div className={`w-4 h-4 border rounded flex items-center justify-center ${
                      selectedTagIds.includes(tag.id) ? 'bg-primary border-primary' : 'border-input'
                    }`}>
                      {selectedTagIds.includes(tag.id) && (
                        <svg className="w-3 h-3 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M5 13l4 4L19 7" />
                        </svg>
                      )}
                    </div>
                    <span 
                      className="w-2 h-2 rounded-full" 
                      style={{ backgroundColor: tag.color || '#6b7280' }}
                    />
                    {tag.name}
                  </button>
                ))
              )}
            </div>
          </div>
          
          {customFields.length > 0 && (
            <div className="space-y-4 pt-2 border-t">
              <Label className="text-sm font-medium text-muted-foreground">Campos Personalizados</Label>
              {customFields.map((field) => (
                <div key={field.id} className="space-y-2">
                  <Label htmlFor={`custom-${field.id}`}>
                    {field.label}
                    {field.isRequired && <span className="text-red-500 ml-1">*</span>}
                  </Label>
                  {field.fieldType === 'TEXT' && (
                    <Input
                      id={`custom-${field.id}`}
                      value={customFieldValues[field.id] || ''}
                      onChange={(e) => setCustomFieldValues({ ...customFieldValues, [field.id]: e.target.value })}
                      required={field.isRequired}
                      data-testid={`input-custom-${field.fieldKey}`}
                    />
                  )}
                  {field.fieldType === 'NUMBER' && (
                    <Input
                      id={`custom-${field.id}`}
                      type="number"
                      value={customFieldValues[field.id] || ''}
                      onChange={(e) => setCustomFieldValues({ ...customFieldValues, [field.id]: e.target.value })}
                      required={field.isRequired}
                      data-testid={`input-custom-${field.fieldKey}`}
                    />
                  )}
                  {field.fieldType === 'DATE' && (
                    <Input
                      id={`custom-${field.id}`}
                      type="date"
                      value={customFieldValues[field.id] || ''}
                      onChange={(e) => setCustomFieldValues({ ...customFieldValues, [field.id]: e.target.value })}
                      required={field.isRequired}
                      data-testid={`input-custom-${field.fieldKey}`}
                    />
                  )}
                  {field.fieldType === 'CHECKBOX' && (
                    <div className="flex items-center gap-2">
                      <Checkbox
                        id={`custom-${field.id}`}
                        checked={customFieldValues[field.id] || false}
                        onCheckedChange={(checked) => setCustomFieldValues({ ...customFieldValues, [field.id]: checked })}
                        data-testid={`checkbox-custom-${field.fieldKey}`}
                      />
                    </div>
                  )}
                  {field.fieldType === 'SELECT' && field.selectOptions && (
                    <Select
                      value={customFieldValues[field.id] || ''}
                      onValueChange={(value) => setCustomFieldValues({ ...customFieldValues, [field.id]: value })}
                    >
                      <SelectTrigger data-testid={`select-custom-${field.fieldKey}`}>
                        <SelectValue placeholder="Selecione..." />
                      </SelectTrigger>
                      <SelectContent>
                        {field.selectOptions.map((option) => (
                          <SelectItem key={option} value={option}>{option}</SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                </div>
              ))}
            </div>
          )}

          <div className="space-y-2">
            <Label htmlFor="notes">Observações</Label>
            <Textarea
              id="notes"
              value={formData.notes}
              onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
              placeholder="Observações sobre o lead..."
              rows={3}
              data-testid="input-lead-notes"
            />
          </div>
          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading} data-testid="button-save-lead">
              {isLoading && <Loader2 className="size-4 mr-2 animate-spin" />}
              {lead ? 'Salvar' : 'Criar'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

interface CleanupModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSuccess: () => void;
}

function CleanupModal({ open, onOpenChange, onSuccess }: CleanupModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [selectedIds, setSelectedIds] = useState<string[]>([]);
  const [activeTab, setActiveTab] = useState('no-contact');

  const { data: leadsWithoutContact = [], isLoading: loadingNoContact, refetch: refetchNoContact } = useQuery({
    queryKey: ['leads-without-contact', currentWorkspace?.id],
    queryFn: () => api.getLeadsWithoutContact(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  const { data: duplicateGroups = [], isLoading: loadingDuplicates, refetch: refetchDuplicates } = useQuery({
    queryKey: ['leads-duplicates', currentWorkspace?.id],
    queryFn: () => api.getDuplicateLeads(currentWorkspace!.id),
    enabled: !!currentWorkspace && open,
  });

  useEffect(() => {
    if (open) {
      setSelectedIds([]);
    }
  }, [open, activeTab]);

  const deleteMutation = useMutation({
    mutationFn: (leadIds: string[]) => api.deleteLeadsBatch(currentWorkspace!.id, leadIds),
    onSuccess: (data) => {
      toast({ title: "Sucesso", description: `${data.deletedCount} leads excluídos!` });
      queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
      refetchNoContact();
      refetchDuplicates();
      setSelectedIds([]);
      onSuccess();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const toggleSelect = (id: string) => {
    setSelectedIds(prev => 
      prev.includes(id) ? prev.filter(i => i !== id) : [...prev, id]
    );
  };

  const selectAll = (ids: string[]) => {
    setSelectedIds(ids);
  };

  const handleDeleteSelected = () => {
    if (selectedIds.length > 0) {
      deleteMutation.mutate(selectedIds);
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl max-h-[80vh] overflow-hidden flex flex-col">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <Sparkles className="size-5" />
            Limpeza de Dados
          </DialogTitle>
        </DialogHeader>
        
        <Tabs value={activeTab} onValueChange={setActiveTab} className="flex-1 overflow-hidden flex flex-col">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="no-contact" className="flex items-center gap-2">
              <AlertTriangle className="size-4" />
              Sem Contato ({leadsWithoutContact.length})
            </TabsTrigger>
            <TabsTrigger value="duplicates" className="flex items-center gap-2">
              <Copy className="size-4" />
              Duplicados ({duplicateGroups.length})
            </TabsTrigger>
          </TabsList>
          
          <TabsContent value="no-contact" className="flex-1 overflow-hidden flex flex-col mt-4">
            <p className="text-sm text-muted-foreground mb-3">
              Leads que não possuem email nem telefone (não podem ser contatados)
            </p>
            {loadingNoContact ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="size-6 animate-spin" />
              </div>
            ) : leadsWithoutContact.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                Nenhum lead sem informações de contato encontrado
              </div>
            ) : (
              <>
                <div className="flex items-center justify-between mb-2">
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => selectAll(leadsWithoutContact.map((l: any) => l.id))}
                  >
                    Selecionar todos ({leadsWithoutContact.length})
                  </Button>
                  {selectedIds.length > 0 && (
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={handleDeleteSelected}
                      disabled={deleteMutation.isPending}
                    >
                      {deleteMutation.isPending ? (
                        <Loader2 className="size-4 mr-2 animate-spin" />
                      ) : (
                        <Trash2 className="size-4 mr-2" />
                      )}
                      Excluir selecionados ({selectedIds.length})
                    </Button>
                  )}
                </div>
                <div className="flex-1 overflow-y-auto border rounded-md">
                  {leadsWithoutContact.map((lead: any) => (
                    <div
                      key={lead.id}
                      className={`flex items-center gap-3 p-3 border-b last:border-b-0 hover:bg-muted/50 ${
                        selectedIds.includes(lead.id) ? 'bg-muted' : ''
                      }`}
                    >
                      <Checkbox
                        checked={selectedIds.includes(lead.id)}
                        onCheckedChange={() => toggleSelect(lead.id)}
                      />
                      <div className="flex-1 min-w-0">
                        <p className="font-medium truncate">{lead.name || 'Sem nome'}</p>
                        <p className="text-sm text-muted-foreground">
                          {sourceLabels[lead.source] || lead.source} • {statusLabels[lead.status] || lead.status}
                        </p>
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}
          </TabsContent>
          
          <TabsContent value="duplicates" className="flex-1 overflow-hidden flex flex-col mt-4">
            <p className="text-sm text-muted-foreground mb-3">
              Grupos de leads com mesmo email ou telefone. Selecione os que deseja excluir (mantenha pelo menos 1 de cada grupo).
            </p>
            {loadingDuplicates ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="size-6 animate-spin" />
              </div>
            ) : duplicateGroups.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                Nenhum lead duplicado encontrado
              </div>
            ) : (
              <>
                {selectedIds.length > 0 && (
                  <div className="flex justify-end mb-2">
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={handleDeleteSelected}
                      disabled={deleteMutation.isPending}
                    >
                      {deleteMutation.isPending ? (
                        <Loader2 className="size-4 mr-2 animate-spin" />
                      ) : (
                        <Trash2 className="size-4 mr-2" />
                      )}
                      Excluir selecionados ({selectedIds.length})
                    </Button>
                  </div>
                )}
                <div className="flex-1 overflow-y-auto space-y-4">
                  {duplicateGroups.map((group: any, groupIndex: number) => (
                    <div key={groupIndex} className="border rounded-md p-3">
                      <p className="text-sm font-medium text-muted-foreground mb-2">
                        {group.email && `Email: ${group.email}`}
                        {group.email && group.phone && ' | '}
                        {group.phone && `Tel: ${group.phone}`}
                        <span className="ml-2">({group.leads.length} leads)</span>
                      </p>
                      <div className="space-y-2">
                        {group.leads.map((lead: any, index: number) => (
                          <div
                            key={lead.id}
                            className={`flex items-center gap-3 p-2 rounded border ${
                              selectedIds.includes(lead.id) ? 'bg-red-50 border-red-200' : 'bg-background'
                            }`}
                          >
                            <Checkbox
                              checked={selectedIds.includes(lead.id)}
                              onCheckedChange={() => toggleSelect(lead.id)}
                            />
                            <div className="flex-1 min-w-0">
                              <p className="font-medium truncate">{lead.name || 'Sem nome'}</p>
                              <div className="flex items-center gap-2 text-xs text-muted-foreground">
                                {lead.email && <span>{lead.email}</span>}
                                {lead.phone && <span>{lead.phone}</span>}
                              </div>
                            </div>
                            {index === 0 && (
                              <Badge variant="outline" className="text-xs">Mais recente</Badge>
                            )}
                          </div>
                        ))}
                      </div>
                    </div>
                  ))}
                </div>
              </>
            )}
          </TabsContent>
        </Tabs>
        
        <DialogFooter className="mt-4">
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Fechar
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

export default function LeadsPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const fileInputRef = useRef<HTMLInputElement>(null);
  
  const [search, setSearch] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);
  const [filterSource, setFilterSource] = useState<string>('');
  const [filterStatus, setFilterStatus] = useState<string>('');
  const [sortBy, setSortBy] = useState<string>('newest');
  const [leadModalOpen, setLeadModalOpen] = useState(false);
  const [editingLead, setEditingLead] = useState<any>(null);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [leadToDelete, setLeadToDelete] = useState<any>(null);
  const [importingGoogle, setImportingGoogle] = useState(false);
  const [cleanupModalOpen, setCleanupModalOpen] = useState(false);

  const sortOptions = [
    { value: 'newest', label: 'Mais recentes' },
    { value: 'oldest', label: 'Mais antigos' },
    { value: 'name_asc', label: 'Nome (A-Z)' },
    { value: 'name_desc', label: 'Nome (Z-A)' },
    { value: 'updated', label: 'Última atualização' },
  ];

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(search);
      setPage(1);
    }, 300);
    return () => clearTimeout(timer);
  }, [search]);

  useEffect(() => {
    setPage(1);
  }, [filterSource, filterStatus]);

  const { data: leadsData, isLoading } = useQuery({
    queryKey: ['leads-paginated', currentWorkspace?.id, page, pageSize, debouncedSearch, filterSource, filterStatus, sortBy],
    queryFn: () => api.getLeadsPaginated(currentWorkspace!.id, {
      page,
      pageSize,
      search: debouncedSearch || undefined,
      source: filterSource || undefined,
      status: filterStatus || undefined,
      sortBy: sortBy || undefined,
    }),
    enabled: !!currentWorkspace,
  });

  const leads = leadsData?.items || [];
  const total = leadsData?.total || 0;
  const totalPages = Math.ceil(total / pageSize);

  const { data: googleStatus } = useQuery({
    queryKey: ['google-contacts-status', currentWorkspace?.id],
    queryFn: () => api.getGoogleContactsStatus(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    if (params.get('google_connected') === 'true') {
      toast({ title: "Sucesso", description: "Google Contacts conectado com sucesso!" });
      window.history.replaceState({}, '', window.location.pathname);
      queryClient.invalidateQueries({ queryKey: ['google-contacts-status'] });
    }
  }, []);

  const deleteMutation = useMutation({
    mutationFn: (leadId: string) => api.deleteLead(currentWorkspace!.id, leadId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
      toast({ title: "Sucesso", description: "Lead excluído com sucesso!" });
      setDeleteModalOpen(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const importGoogleMutation = useMutation({
    mutationFn: () => api.importGoogleContacts(currentWorkspace!.id),
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
      toast({ title: "Sucesso", description: `${data.imported} contatos importados do Google!` });
      setImportingGoogle(false);
    },
    onError: async (error: any) => {
      if (error.needsAuth) {
        try {
          const { authUrl } = await api.getGoogleContactsAuthUrl(currentWorkspace!.id);
          window.location.href = authUrl;
        } catch (authError: any) {
          toast({ title: "Erro", description: authError.message, variant: "destructive" });
        }
      } else {
        toast({ title: "Erro", description: error.message, variant: "destructive" });
      }
      setImportingGoogle(false);
    },
  });

  const connectGoogleMutation = useMutation({
    mutationFn: () => api.getGoogleContactsAuthUrl(currentWorkspace!.id),
    onSuccess: ({ authUrl }) => {
      window.location.href = authUrl;
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const disconnectGoogleMutation = useMutation({
    mutationFn: () => api.disconnectGoogleContacts(currentWorkspace!.id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['google-contacts-status'] });
      toast({ title: "Sucesso", description: "Google Contacts desconectado." });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleExport = async () => {
    try {
      const blob = await api.exportLeads(currentWorkspace!.id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'leads.csv';
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
      toast({ title: "Sucesso", description: "Leads exportados com sucesso!" });
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    }
  };

  const handleFileImport = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    try {
      const text = await file.text();
      const lines = text.split('\n').filter(line => line.trim());
      
      const importedLeads = lines.slice(1).map(line => {
        const values = line.split(',');
        return {
          name: values[0]?.trim() || '',
          email: values[1]?.trim() || '',
          phone: values[2]?.trim() || '',
        };
      }).filter(lead => lead.name || lead.email);

      const result = await api.importLeads(currentWorkspace!.id, importedLeads);
      queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
      toast({ title: "Sucesso", description: `${result.imported} leads importados!` });
    } catch (error: any) {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    }
    
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleGoogleImport = async () => {
    if (!googleStatus?.connected) {
      connectGoogleMutation.mutate();
      return;
    }
    setImportingGoogle(true);
    importGoogleMutation.mutate();
  };

  const clearFilters = () => {
    setSearch('');
    setFilterSource('');
    setFilterStatus('');
    setPage(1);
  };

  const hasActiveFilters = search || filterSource || filterStatus;

  const openEditModal = (lead: any) => {
    setEditingLead(lead);
    setLeadModalOpen(true);
  };

  const openDeleteModal = (lead: any) => {
    setLeadToDelete(lead);
    setDeleteModalOpen(true);
  };

  const handleModalSuccess = () => {
    queryClient.invalidateQueries({ queryKey: ['leads-paginated'] });
    setEditingLead(null);
  };

  const startItem = total === 0 ? 0 : (page - 1) * pageSize + 1;
  const endItem = Math.min(page * pageSize, total);

  if (!currentWorkspace) return null;

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight">Leads</h1>
            <p className="text-muted-foreground">Gerencie seus leads e potenciais clientes</p>
          </div>
          <div className="flex items-center gap-2">
            <input
              ref={fileInputRef}
              type="file"
              accept=".csv"
              onChange={handleFileImport}
              className="hidden"
            />
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="outline" data-testid="button-import-leads">
                  <Upload className="size-4 mr-2" />
                  Importar
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent>
                <DropdownMenuItem onClick={() => fileInputRef.current?.click()}>
                  <Upload className="size-4 mr-2" />
                  Importar CSV
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                {googleStatus?.connected ? (
                  <>
                    <DropdownMenuItem onClick={handleGoogleImport} disabled={importingGoogle}>
                      {importingGoogle ? (
                        <Loader2 className="size-4 mr-2 animate-spin" />
                      ) : (
                        <Users className="size-4 mr-2" />
                      )}
                      Importar do Google Contacts
                    </DropdownMenuItem>
                    <DropdownMenuItem 
                      onClick={() => disconnectGoogleMutation.mutate()}
                      className="text-muted-foreground"
                    >
                      <Link2Off className="size-4 mr-2" />
                      Desconectar Google ({googleStatus.email})
                    </DropdownMenuItem>
                  </>
                ) : (
                  <DropdownMenuItem onClick={handleGoogleImport} disabled={connectGoogleMutation.isPending}>
                    {connectGoogleMutation.isPending ? (
                      <Loader2 className="size-4 mr-2 animate-spin" />
                    ) : (
                      <Link2 className="size-4 mr-2" />
                    )}
                    Conectar Google Contacts
                  </DropdownMenuItem>
                )}
              </DropdownMenuContent>
            </DropdownMenu>
            <Button variant="outline" onClick={handleExport} data-testid="button-export-leads">
              <Download className="size-4 mr-2" />
              Exportar
            </Button>
            <Button variant="outline" onClick={() => setCleanupModalOpen(true)} data-testid="button-cleanup-leads">
              <Sparkles className="size-4 mr-2" />
              Limpar Dados
            </Button>
            <Button onClick={() => { setEditingLead(null); setLeadModalOpen(true); }} data-testid="button-new-lead">
              <Plus className="size-4 mr-2" />
              Novo Lead
            </Button>
          </div>
        </div>

        <div className="flex flex-col sm:flex-row gap-3">
          <div className="relative flex-1 max-w-md">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
            <Input
              placeholder="Buscar leads..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="pl-9"
              data-testid="input-search-leads"
            />
            {search && (
              <Button
                variant="ghost"
                size="icon"
                className="absolute right-1 top-1/2 -translate-y-1/2 size-6"
                onClick={() => setSearch('')}
              >
                <X className="size-3" />
              </Button>
            )}
          </div>
          <div className="flex flex-wrap gap-2">
            <Select value={filterSource} onValueChange={setFilterSource}>
              <SelectTrigger className="w-[160px]" data-testid="filter-source">
                <Filter className="size-4 mr-2" />
                <SelectValue placeholder="Origem" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Todas origens</SelectItem>
                {Object.entries(sourceLabels).map(([key, label]) => (
                  <SelectItem key={key} value={key}>{label}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            <Select value={filterStatus} onValueChange={setFilterStatus}>
              <SelectTrigger className="w-[160px]" data-testid="filter-status">
                <Filter className="size-4 mr-2" />
                <SelectValue placeholder="Status" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Todos status</SelectItem>
                {Object.entries(statusLabels).map(([key, label]) => (
                  <SelectItem key={key} value={key}>{label}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            <Select value={sortBy} onValueChange={setSortBy}>
              <SelectTrigger className="w-[180px]" data-testid="sort-leads">
                <ArrowUpDown className="size-4 mr-2" />
                <SelectValue placeholder="Ordenar por" />
              </SelectTrigger>
              <SelectContent>
                {sortOptions.map((option) => (
                  <SelectItem key={option.value} value={option.value}>{option.label}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            {hasActiveFilters && (
              <Button variant="ghost" onClick={clearFilters} data-testid="button-clear-filters">
                <X className="size-4 mr-2" />
                Limpar filtros
              </Button>
            )}
          </div>
        </div>

        {isLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {[1, 2, 3, 4, 5, 6].map((i) => (
              <TactileCard key={i} className="p-5">
                <div className="flex items-start gap-4">
                  <Skeleton className="size-12 rounded-full" />
                  <div className="flex-1 space-y-2">
                    <Skeleton className="h-5 w-32" />
                    <Skeleton className="h-4 w-48" />
                    <Skeleton className="h-4 w-24" />
                  </div>
                </div>
              </TactileCard>
            ))}
          </div>
        ) : leads.length === 0 ? (
          <TactileCard className="p-12 text-center">
            <Users className="size-12 mx-auto text-muted-foreground mb-4" />
            <h3 className="text-lg font-semibold mb-2">
              {hasActiveFilters ? 'Nenhum lead encontrado' : 'Nenhum lead ainda'}
            </h3>
            <p className="text-muted-foreground mb-4">
              {hasActiveFilters 
                ? 'Tente ajustar os filtros ou busca' 
                : 'Comece adicionando seu primeiro lead ou importe do Google Contacts'}
            </p>
            {!hasActiveFilters && (
              <div className="flex items-center justify-center gap-2">
                <Button onClick={() => setLeadModalOpen(true)}>
                  <Plus className="size-4 mr-2" />
                  Novo Lead
                </Button>
                <Button variant="outline" onClick={handleGoogleImport} disabled={importingGoogle}>
                  {importingGoogle ? (
                    <Loader2 className="size-4 mr-2 animate-spin" />
                  ) : (
                    <Users className="size-4 mr-2" />
                  )}
                  Importar do Google
                </Button>
              </div>
            )}
          </TactileCard>
        ) : (
          <>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              {leads.map((lead: any) => (
                <TactileCard 
                  key={lead.id} 
                  className="p-5 hover:shadow-md transition-shadow"
                  data-testid={`card-lead-${lead.id}`}
                >
                  <div className="flex items-start justify-between">
                    <div className="flex items-start gap-3">
                      <Avatar className="size-12">
                        <AvatarFallback className="bg-primary/10 text-primary font-semibold">
                          {(lead.name || lead.email || 'L')[0].toUpperCase()}
                        </AvatarFallback>
                      </Avatar>
                      <div className="space-y-1">
                        <h3 className="font-semibold line-clamp-1">{lead.name || 'Sem nome'}</h3>
                        {lead.email && (
                          <div className="flex items-center gap-1 text-sm text-muted-foreground">
                            <Mail className="size-3" />
                            <span className="line-clamp-1">{lead.email}</span>
                          </div>
                        )}
                        {lead.phone && (
                          <div className="flex items-center gap-1 text-sm text-muted-foreground">
                            <Phone className="size-3" />
                            <span>{lead.phone}</span>
                          </div>
                        )}
                      </div>
                    </div>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon" className="size-8">
                          <MoreHorizontal className="size-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuItem onClick={() => openEditModal(lead)}>
                          <Pencil className="size-4 mr-2" />
                          Editar
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem 
                          onClick={() => openDeleteModal(lead)}
                          className="text-destructive focus:text-destructive"
                        >
                          <Trash2 className="size-4 mr-2" />
                          Excluir
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                  <div className="flex flex-wrap items-center gap-2 mt-4">
                    <Badge className={statusColors[lead.status] || 'bg-gray-100 text-gray-800'}>
                      {statusLabels[lead.status] || lead.status}
                    </Badge>
                    {lead.source && (
                      <Badge variant="outline" className="text-xs">
                        {lead.source === 'BOOKING' && <Calendar className="size-3 mr-1" />}
                        {sourceLabels[lead.source] || lead.source}
                      </Badge>
                    )}
                    {lead.tags?.map((tag: LeadTag) => (
                      <Badge 
                        key={tag.id} 
                        variant="secondary" 
                        className="text-xs"
                        style={{ 
                          backgroundColor: tag.color ? `${tag.color}20` : undefined,
                          color: tag.color || undefined,
                          borderColor: tag.color || undefined,
                        }}
                        data-testid={`tag-badge-${tag.id}`}
                      >
                        <Tag className="size-3 mr-1" />
                        {tag.name}
                      </Badge>
                    ))}
                  </div>
                </TactileCard>
              ))}
            </div>

            {totalPages > 1 && (
              <div className="flex flex-col sm:flex-row items-center justify-between gap-4 pt-4 border-t">
                <span className="text-sm text-muted-foreground" data-testid="pagination-info">
                  Mostrando {startItem}-{endItem} de {total} leads
                </span>
                <div className="flex items-center gap-2">
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => setPage(p => Math.max(1, p - 1))}
                    disabled={page === 1}
                    data-testid="button-prev-page"
                  >
                    <ChevronLeft className="size-4 mr-1" />
                    Anterior
                  </Button>
                  <div className="flex items-center gap-1">
                    {Array.from({ length: Math.min(5, totalPages) }, (_, i) => {
                      let pageNum: number;
                      if (totalPages <= 5) {
                        pageNum = i + 1;
                      } else if (page <= 3) {
                        pageNum = i + 1;
                      } else if (page >= totalPages - 2) {
                        pageNum = totalPages - 4 + i;
                      } else {
                        pageNum = page - 2 + i;
                      }
                      return (
                        <Button
                          key={pageNum}
                          variant={pageNum === page ? "default" : "outline"}
                          size="sm"
                          className="w-8"
                          onClick={() => setPage(pageNum)}
                          data-testid={`button-page-${pageNum}`}
                        >
                          {pageNum}
                        </Button>
                      );
                    })}
                  </div>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                    disabled={page === totalPages}
                    data-testid="button-next-page"
                  >
                    Próximo
                    <ChevronRight className="size-4 ml-1" />
                  </Button>
                </div>
              </div>
            )}
          </>
        )}
      </div>

      <LeadModal
        open={leadModalOpen}
        onOpenChange={(open) => {
          setLeadModalOpen(open);
          if (!open) setEditingLead(null);
        }}
        lead={editingLead}
        onSuccess={handleModalSuccess}
      />

      <DeleteModal
        open={deleteModalOpen}
        onOpenChange={setDeleteModalOpen}
        title="Excluir Lead"
        description={`Tem certeza que deseja excluir o lead "${leadToDelete?.name || leadToDelete?.email}"? Esta ação não pode ser desfeita.`}
        onConfirm={() => leadToDelete && deleteMutation.mutate(leadToDelete.id)}
        isLoading={deleteMutation.isPending}
      />

      <CleanupModal
        open={cleanupModalOpen}
        onOpenChange={setCleanupModalOpen}
        onSuccess={handleModalSuccess}
      />
    </AppShell>
  );
}
