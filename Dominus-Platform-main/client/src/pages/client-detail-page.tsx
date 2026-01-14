import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams, useLocation, Link } from "wouter";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Label } from "@/components/ui/label";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ScrollArea } from "@/components/ui/scroll-area";
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
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import {
  ArrowLeft, Pencil, Mail, Phone, Building2, User, MapPin, 
  FolderKanban, ClipboardList, FileText, Receipt, MessageSquare,
  Send, Loader2, Calendar, DollarSign, CheckCircle,
  Clock, FileUp, MoreHorizontal, Download, Trash2, Plus, X, Files
} from "lucide-react";
import { format, differenceInDays, parseISO } from "date-fns";
import { ptBR } from "date-fns/locale";
import { ObjectUploader } from "@/components/ObjectUploader";

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value);
}

function formatCPF(cpf: string): string {
  if (!cpf) return '';
  const digits = cpf.replace(/\D/g, '');
  return digits
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
}

function formatCNPJ(cnpj: string): string {
  if (!cnpj) return '';
  const digits = cnpj.replace(/\D/g, '');
  return digits
    .replace(/(\d{2})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1/$2')
    .replace(/(\d{4})(\d{1,2})$/, '$1-$2');
}

const DOCUMENT_TYPES = [
  { value: 'CONTRACT', label: 'Contrato' },
  { value: 'LICENSE', label: 'Licença' },
  { value: 'CERTIFICATE', label: 'Certificado' },
  { value: 'PERMIT', label: 'Alvará' },
  { value: 'ID', label: 'Identidade' },
  { value: 'OTHER', label: 'Outro' },
];

function getDocumentTypeLabel(type: string): string {
  const found = DOCUMENT_TYPES.find(t => t.value === type);
  return found ? found.label : type;
}

function getDocumentStatus(expirationDate: string | null, alertDaysBefore: number = 30): { status: string; label: string; variant: 'default' | 'secondary' | 'destructive' | 'outline' } {
  if (!expirationDate) {
    return { status: 'VALID', label: 'Válido', variant: 'default' };
  }
  
  const today = new Date();
  const expDate = parseISO(expirationDate);
  const daysUntilExpiration = differenceInDays(expDate, today);
  
  if (daysUntilExpiration < 0) {
    return { status: 'EXPIRED', label: 'Expirado', variant: 'destructive' };
  } else if (daysUntilExpiration <= alertDaysBefore) {
    return { status: 'EXPIRING_SOON', label: 'Expirando', variant: 'secondary' };
  }
  return { status: 'VALID', label: 'Válido', variant: 'default' };
}

interface DocumentFormData {
  name: string;
  description: string;
  documentType: string;
  documentNumber: string;
  issueDate: string;
  expirationDate: string;
  alertDaysBefore: number;
  notes: string;
}

const initialFormData: DocumentFormData = {
  name: '',
  description: '',
  documentType: 'OTHER',
  documentNumber: '',
  issueDate: '',
  expirationDate: '',
  alertDaysBefore: 30,
  notes: '',
};

export default function ClientDetailPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [, navigate] = useLocation();
  const params = useParams<{ slug: string; id: string }>();
  const clientId = params.id;

  const [messageForm, setMessageForm] = useState({
    subject: "",
    content: "",
    channel: "EMAIL",
  });

  const [documentModalOpen, setDocumentModalOpen] = useState(false);
  const [editingDocument, setEditingDocument] = useState<any>(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [documentToDelete, setDocumentToDelete] = useState<any>(null);
  const [documentForm, setDocumentForm] = useState<DocumentFormData>(initialFormData);
  const [uploadedFile, setUploadedFile] = useState<{ url: string; name: string; size: number; type: string } | null>(null);

  const { data: data360, isLoading } = useQuery({
    queryKey: ['client360', currentWorkspace?.id, clientId],
    queryFn: () => api.getClient360(currentWorkspace!.id, clientId!),
    enabled: !!currentWorkspace && !!clientId,
  });

  const { data: documents = [], isLoading: isLoadingDocuments } = useQuery({
    queryKey: ['clientDocuments', currentWorkspace?.id, clientId],
    queryFn: () => api.getClientDocuments(currentWorkspace!.id, clientId!),
    enabled: !!currentWorkspace && !!clientId,
  });

  const sendMessageMutation = useMutation({
    mutationFn: (data: any) => api.createClientMessage(currentWorkspace!.id, clientId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client360', currentWorkspace?.id, clientId] });
      toast({ title: "Sucesso", description: "Mensagem enviada com sucesso!" });
      setMessageForm({ subject: "", content: "", channel: "EMAIL" });
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const createDocumentMutation = useMutation({
    mutationFn: (data: any) => api.createClientDocument(currentWorkspace!.id, clientId!, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientDocuments', currentWorkspace?.id, clientId] });
      toast({ title: "Sucesso", description: "Documento adicionado com sucesso!" });
      resetDocumentForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateDocumentMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: any }) => api.updateClientDocument(currentWorkspace!.id, id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientDocuments', currentWorkspace?.id, clientId] });
      toast({ title: "Sucesso", description: "Documento atualizado com sucesso!" });
      resetDocumentForm();
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteDocumentMutation = useMutation({
    mutationFn: (id: string) => api.deleteClientDocument(currentWorkspace!.id, id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientDocuments', currentWorkspace?.id, clientId] });
      toast({ title: "Sucesso", description: "Documento excluído com sucesso!" });
      setDeleteDialogOpen(false);
      setDocumentToDelete(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSendMessage = () => {
    if (!messageForm.content.trim()) {
      toast({ title: "Erro", description: "Digite o conteúdo da mensagem", variant: "destructive" });
      return;
    }
    sendMessageMutation.mutate(messageForm);
  };

  const resetDocumentForm = () => {
    setDocumentForm(initialFormData);
    setUploadedFile(null);
    setEditingDocument(null);
    setDocumentModalOpen(false);
  };

  const handleEditDocument = (doc: any) => {
    setEditingDocument(doc);
    setDocumentForm({
      name: doc.name || '',
      description: doc.description || '',
      documentType: doc.documentType || 'OTHER',
      documentNumber: doc.documentNumber || '',
      issueDate: doc.issueDate || '',
      expirationDate: doc.expirationDate || '',
      alertDaysBefore: doc.alertDaysBefore || 30,
      notes: doc.notes || '',
    });
    setUploadedFile({
      url: doc.fileUrl,
      name: doc.fileName,
      size: doc.fileSize || 0,
      type: doc.mimeType || '',
    });
    setDocumentModalOpen(true);
  };

  const handleDeleteDocument = (doc: any) => {
    setDocumentToDelete(doc);
    setDeleteDialogOpen(true);
  };

  const handleSubmitDocument = () => {
    if (!documentForm.name.trim()) {
      toast({ title: "Erro", description: "Nome do documento é obrigatório", variant: "destructive" });
      return;
    }
    
    if (!editingDocument && !uploadedFile) {
      toast({ title: "Erro", description: "Faça upload de um arquivo", variant: "destructive" });
      return;
    }

    const data = {
      name: documentForm.name,
      description: documentForm.description || undefined,
      documentType: documentForm.documentType,
      documentNumber: documentForm.documentNumber || undefined,
      issueDate: documentForm.issueDate || undefined,
      expirationDate: documentForm.expirationDate || undefined,
      alertDaysBefore: documentForm.alertDaysBefore,
      notes: documentForm.notes || undefined,
      ...(uploadedFile && {
        fileName: uploadedFile.name,
        fileUrl: uploadedFile.url,
        fileSize: uploadedFile.size,
        mimeType: uploadedFile.type,
      }),
    };

    if (editingDocument) {
      updateDocumentMutation.mutate({ id: editingDocument.id, data });
    } else {
      createDocumentMutation.mutate(data);
    }
  };

  const handleUploadComplete = (result: any) => {
    if (result.successful && result.successful.length > 0) {
      const file = result.successful[0];
      setUploadedFile({
        url: file.uploadURL || file.response?.body?.url || '',
        name: file.name,
        size: file.size,
        type: file.type,
      });
      toast({ title: "Sucesso", description: "Arquivo carregado com sucesso!" });
    }
  };

  const getUploadParameters = async (file: any) => {
    const response = await fetch(`/api/workspaces/${currentWorkspace!.id}/clients/${clientId}/documents/request-url`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({
        name: file.name,
        size: file.size,
        contentType: file.type || "application/octet-stream",
      }),
    });
    
    if (!response.ok) {
      throw new Error("Falha ao obter URL de upload");
    }
    
    const data = await response.json();
    
    setUploadedFile({
      url: data.objectPath || data.uploadUrl,
      name: file.name,
      size: file.size || 0,
      type: file.type || "application/octet-stream",
    });
    
    return {
      method: "PUT" as const,
      url: data.uploadUrl,
      headers: { "Content-Type": file.type || "application/octet-stream" },
    };
  };

  if (!currentWorkspace?.slug) {
    return null;
  }

  const client = data360?.client;
  const stats = data360?.stats;

  return (
    <AppShell>
      <div className="p-6 max-w-7xl mx-auto space-y-6">
        <div className="flex items-center gap-4">
          <Button 
            variant="ghost" 
            size="icon"
            onClick={() => navigate(`/${currentWorkspace.slug}/clients`)}
            data-testid="button-back"
          >
            <ArrowLeft className="size-5" />
          </Button>
          <div className="flex-1">
            <h1 className="text-2xl font-bold">
              {isLoading ? <Skeleton className="h-8 w-48" /> : client?.name}
            </h1>
            <p className="text-muted-foreground">
              {isLoading ? <Skeleton className="h-4 w-32 mt-1" /> : 'Visão 360° do Cliente'}
            </p>
          </div>
          <Link href={`/${currentWorkspace.slug}/clients/${clientId}/edit`}>
            <Button data-testid="button-edit-client">
              <Pencil className="size-4 mr-2" />
              Editar Cliente
            </Button>
          </Link>
        </div>

        {isLoading ? (
          <div className="grid gap-6 md:grid-cols-4">
            {[...Array(4)].map((_, i) => (
              <Skeleton key={i} className="h-24 rounded-xl" />
            ))}
          </div>
        ) : (
          <>
            <div className="grid gap-4 md:grid-cols-4">
              <TactileCard className="p-4" data-testid="stat-revenue">
                <div className="flex items-center gap-3">
                  <div className="p-2 rounded-lg bg-green-500/10">
                    <DollarSign className="size-5 text-green-500" />
                  </div>
                  <div>
                    <p className="text-sm text-muted-foreground">Receita Total</p>
                    <p className="text-xl font-bold">{formatCurrency(stats?.totalRevenue || 0)}</p>
                  </div>
                </div>
              </TactileCard>

              <TactileCard className="p-4" data-testid="stat-projects">
                <div className="flex items-center gap-3">
                  <div className="p-2 rounded-lg bg-blue-500/10">
                    <FolderKanban className="size-5 text-blue-500" />
                  </div>
                  <div>
                    <p className="text-sm text-muted-foreground">Projetos</p>
                    <p className="text-xl font-bold">{stats?.activeProjects || 0} <span className="text-sm font-normal text-muted-foreground">ativos</span></p>
                  </div>
                </div>
              </TactileCard>

              <TactileCard className="p-4" data-testid="stat-tasks">
                <div className="flex items-center gap-3">
                  <div className="p-2 rounded-lg bg-orange-500/10">
                    <ClipboardList className="size-5 text-orange-500" />
                  </div>
                  <div>
                    <p className="text-sm text-muted-foreground">Tarefas Abertas</p>
                    <p className="text-xl font-bold">{stats?.openTasks || 0}</p>
                  </div>
                </div>
              </TactileCard>

              <TactileCard className="p-4" data-testid="stat-invoices">
                <div className="flex items-center gap-3">
                  <div className="p-2 rounded-lg bg-purple-500/10">
                    <Receipt className="size-5 text-purple-500" />
                  </div>
                  <div>
                    <p className="text-sm text-muted-foreground">Faturas Pagas</p>
                    <p className="text-xl font-bold">{stats?.paidInvoices || 0} / {stats?.totalInvoices || 0}</p>
                  </div>
                </div>
              </TactileCard>
            </div>

            <div className="grid gap-6 lg:grid-cols-3">
              <TactileCard className="p-6 lg:col-span-1" data-testid="card-client-info">
                <h2 className="text-lg font-semibold mb-4 flex items-center gap-2">
                  {client?.clientType === 'COMPANY' ? <Building2 className="size-5" /> : <User className="size-5" />}
                  Informações do Cliente
                </h2>
                
                <div className="space-y-4">
                  <div className="flex items-center gap-3">
                    <Avatar className="size-16">
                      <AvatarImage src={client?.avatar} />
                      <AvatarFallback className="text-xl">{client?.name?.charAt(0)}</AvatarFallback>
                    </Avatar>
                    <div>
                      <p className="font-medium">{client?.name}</p>
                      <Badge variant={client?.status === 'ACTIVE' ? 'default' : 'secondary'}>
                        {client?.status === 'ACTIVE' ? 'Ativo' : 'Inativo'}
                      </Badge>
                    </div>
                  </div>

                  <div className="space-y-2 text-sm">
                    {client?.email && (
                      <div className="flex items-center gap-2 text-muted-foreground">
                        <Mail className="size-4" />
                        <span>{client.email}</span>
                      </div>
                    )}
                    {client?.phone && (
                      <div className="flex items-center gap-2 text-muted-foreground">
                        <Phone className="size-4" />
                        <span>{client.phone}</span>
                      </div>
                    )}
                    {(client?.cnpj || client?.cpf) && (
                      <div className="flex items-center gap-2 text-muted-foreground">
                        <FileText className="size-4" />
                        <span>{client.cnpj ? formatCNPJ(client.cnpj) : formatCPF(client.cpf)}</span>
                      </div>
                    )}
                    {client?.city && (
                      <div className="flex items-center gap-2 text-muted-foreground">
                        <MapPin className="size-4" />
                        <span>{client.city}{client.state ? `, ${client.state}` : ''}</span>
                      </div>
                    )}
                  </div>

                  {data360?.contacts?.length > 0 && (
                    <div className="pt-4 border-t">
                      <h3 className="text-sm font-medium mb-2">Contatos</h3>
                      <div className="space-y-2">
                        {data360.contacts.slice(0, 3).map((contact: any) => (
                          <div key={contact.id} className="text-sm flex items-center justify-between">
                            <span>{contact.name}</span>
                            {contact.isPrimary && <Badge variant="outline" className="text-xs">Principal</Badge>}
                          </div>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
              </TactileCard>

              <TactileCard className="p-6 lg:col-span-2" data-testid="card-client-tabs">
                <Tabs defaultValue="projects" className="w-full">
                  <TabsList className="mb-4 flex-wrap h-auto gap-1">
                    <TabsTrigger value="projects" className="gap-2">
                      <FolderKanban className="size-4" />
                      Projetos
                    </TabsTrigger>
                    <TabsTrigger value="tasks" className="gap-2">
                      <ClipboardList className="size-4" />
                      Tarefas
                    </TabsTrigger>
                    <TabsTrigger value="proposals" className="gap-2">
                      <FileText className="size-4" />
                      Propostas
                    </TabsTrigger>
                    <TabsTrigger value="invoices" className="gap-2">
                      <Receipt className="size-4" />
                      Faturas
                    </TabsTrigger>
                    <TabsTrigger value="documents" className="gap-2" data-testid="tab-documents">
                      <Files className="size-4" />
                      Documentos
                    </TabsTrigger>
                  </TabsList>

                  <TabsContent value="projects" className="mt-0">
                    <ScrollArea className="h-[300px]">
                      {data360?.activeProjects?.length > 0 ? (
                        <div className="space-y-3">
                          {data360.activeProjects.map((project: any) => (
                            <div key={project.id} className="p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors">
                              <div className="flex items-center justify-between">
                                <div>
                                  <p className="font-medium">{project.title}</p>
                                  <p className="text-sm text-muted-foreground">{project.description?.substring(0, 50)}</p>
                                </div>
                                <Badge variant={project.status === 'IN_PROGRESS' ? 'default' : 'secondary'}>
                                  {project.status === 'IN_PROGRESS' ? 'Em Andamento' : project.status}
                                </Badge>
                              </div>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <div className="h-full flex items-center justify-center text-muted-foreground">
                          Nenhum projeto ativo
                        </div>
                      )}
                    </ScrollArea>
                  </TabsContent>

                  <TabsContent value="tasks" className="mt-0">
                    <ScrollArea className="h-[300px]">
                      {data360?.openTasks?.length > 0 ? (
                        <div className="space-y-3">
                          {data360.openTasks.map((task: any) => (
                            <div key={task.id} className="p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors">
                              <div className="flex items-center justify-between">
                                <div>
                                  <p className="font-medium">{task.title}</p>
                                  {task.dueDate && (
                                    <p className="text-sm text-muted-foreground flex items-center gap-1">
                                      <Clock className="size-3" />
                                      {format(new Date(task.dueDate), "d 'de' MMM", { locale: ptBR })}
                                    </p>
                                  )}
                                </div>
                                <CheckCircle className="size-5 text-muted-foreground" />
                              </div>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <div className="h-full flex items-center justify-center text-muted-foreground">
                          Nenhuma tarefa aberta
                        </div>
                      )}
                    </ScrollArea>
                  </TabsContent>

                  <TabsContent value="proposals" className="mt-0">
                    <ScrollArea className="h-[300px]">
                      {data360?.openProposals?.length > 0 ? (
                        <div className="space-y-3">
                          {data360.openProposals.map((proposal: any) => (
                            <div key={proposal.id} className="p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors">
                              <div className="flex items-center justify-between">
                                <div>
                                  <p className="font-medium">{proposal.title}</p>
                                  {proposal.validUntil && (
                                    <p className="text-sm text-muted-foreground">
                                      Válida até {format(new Date(proposal.validUntil), "d 'de' MMM", { locale: ptBR })}
                                    </p>
                                  )}
                                </div>
                                <Badge variant={proposal.status === 'SENT' ? 'default' : 'secondary'}>
                                  {proposal.status === 'SENT' ? 'Enviada' : 'Rascunho'}
                                </Badge>
                              </div>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <div className="h-full flex items-center justify-center text-muted-foreground">
                          Nenhuma proposta em aberto
                        </div>
                      )}
                    </ScrollArea>
                  </TabsContent>

                  <TabsContent value="invoices" className="mt-0">
                    <ScrollArea className="h-[300px]">
                      {data360?.pendingInvoices?.length > 0 ? (
                        <div className="space-y-3">
                          {data360.pendingInvoices.map((invoice: any) => (
                            <div key={invoice.id} className="p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors">
                              <div className="flex items-center justify-between">
                                <div>
                                  <p className="font-medium">Fatura #{invoice.number}</p>
                                  <p className="text-sm text-muted-foreground flex items-center gap-1">
                                    <Calendar className="size-3" />
                                    Vence em {format(new Date(invoice.dueDate), "d 'de' MMM", { locale: ptBR })}
                                  </p>
                                </div>
                                <Badge variant={invoice.status === 'OVERDUE' ? 'destructive' : 'default'}>
                                  {invoice.status === 'OVERDUE' ? 'Atrasada' : 'Enviada'}
                                </Badge>
                              </div>
                            </div>
                          ))}
                        </div>
                      ) : (
                        <div className="h-full flex items-center justify-center text-muted-foreground">
                          Nenhuma fatura pendente
                        </div>
                      )}
                    </ScrollArea>
                  </TabsContent>

                  <TabsContent value="documents" className="mt-0">
                    <div className="flex items-center justify-between mb-3">
                      <p className="text-sm text-muted-foreground">
                        {documents.length} documento{documents.length !== 1 ? 's' : ''}
                      </p>
                      <Button 
                        size="sm" 
                        onClick={() => {
                          resetDocumentForm();
                          setDocumentModalOpen(true);
                        }}
                        data-testid="button-add-document"
                      >
                        <Plus className="size-4 mr-1" />
                        Adicionar
                      </Button>
                    </div>
                    <ScrollArea className="h-[260px]">
                      {isLoadingDocuments ? (
                        <div className="space-y-3">
                          {[1, 2, 3].map(i => (
                            <Skeleton key={i} className="h-16 rounded-lg" />
                          ))}
                        </div>
                      ) : documents.length > 0 ? (
                        <div className="space-y-3">
                          {documents.map((doc: any) => {
                            const statusInfo = getDocumentStatus(doc.expirationDate, doc.alertDaysBefore);
                            return (
                              <div 
                                key={doc.id} 
                                className="p-3 rounded-lg border bg-card hover:bg-accent/50 transition-colors"
                                data-testid={`document-${doc.id}`}
                              >
                                <div className="flex items-center justify-between gap-2">
                                  <div className="flex-1 min-w-0">
                                    <div className="flex items-center gap-2 flex-wrap">
                                      <p className="font-medium truncate">{doc.name}</p>
                                      <Badge variant="outline" className="text-xs shrink-0">
                                        {getDocumentTypeLabel(doc.documentType)}
                                      </Badge>
                                      <Badge variant={statusInfo.variant} className="text-xs shrink-0">
                                        {statusInfo.label}
                                      </Badge>
                                    </div>
                                    {doc.expirationDate && (
                                      <p className="text-sm text-muted-foreground flex items-center gap-1 mt-1">
                                        <Calendar className="size-3" />
                                        Vence em {format(parseISO(doc.expirationDate), "d 'de' MMM yyyy", { locale: ptBR })}
                                      </p>
                                    )}
                                  </div>
                                  <DropdownMenu>
                                    <DropdownMenuTrigger asChild>
                                      <Button variant="ghost" size="icon" className="shrink-0">
                                        <MoreHorizontal className="size-4" />
                                      </Button>
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent align="end">
                                      <DropdownMenuItem asChild>
                                        <a 
                                          href={doc.fileUrl} 
                                          target="_blank" 
                                          rel="noopener noreferrer"
                                          className="flex items-center"
                                        >
                                          <Download className="size-4 mr-2" />
                                          Baixar
                                        </a>
                                      </DropdownMenuItem>
                                      <DropdownMenuItem onClick={() => handleEditDocument(doc)}>
                                        <Pencil className="size-4 mr-2" />
                                        Editar
                                      </DropdownMenuItem>
                                      <DropdownMenuItem 
                                        onClick={() => handleDeleteDocument(doc)}
                                        className="text-destructive"
                                      >
                                        <Trash2 className="size-4 mr-2" />
                                        Excluir
                                      </DropdownMenuItem>
                                    </DropdownMenuContent>
                                  </DropdownMenu>
                                </div>
                              </div>
                            );
                          })}
                        </div>
                      ) : (
                        <div className="h-full flex flex-col items-center justify-center text-muted-foreground py-8">
                          <Files className="size-12 mb-3 opacity-50" />
                          <p>Nenhum documento cadastrado</p>
                          <p className="text-sm">Clique em "Adicionar" para começar</p>
                        </div>
                      )}
                    </ScrollArea>
                  </TabsContent>
                </Tabs>
              </TactileCard>
            </div>

            <TactileCard className="p-6" data-testid="card-messages">
              <h2 className="text-lg font-semibold mb-4 flex items-center gap-2">
                <MessageSquare className="size-5" />
                Comunicações
              </h2>

              <div className="grid gap-6 lg:grid-cols-2">
                <div>
                  <h3 className="text-sm font-medium mb-3">Nova Mensagem</h3>
                  <div className="space-y-3">
                    <Input
                      placeholder="Assunto"
                      value={messageForm.subject}
                      onChange={(e) => setMessageForm({ ...messageForm, subject: e.target.value })}
                      data-testid="input-message-subject"
                    />
                    <Textarea
                      placeholder="Escreva sua mensagem..."
                      value={messageForm.content}
                      onChange={(e) => setMessageForm({ ...messageForm, content: e.target.value })}
                      rows={4}
                      data-testid="input-message-content"
                    />
                    <div className="flex items-center gap-2">
                      <Badge variant="outline" className="gap-1">
                        <Mail className="size-3" />
                        Email
                      </Badge>
                      <span className="text-xs text-muted-foreground">SMS e WhatsApp em breve</span>
                    </div>
                    <Button 
                      onClick={handleSendMessage} 
                      disabled={sendMessageMutation.isPending}
                      className="w-full"
                      data-testid="button-send-message"
                    >
                      {sendMessageMutation.isPending ? (
                        <Loader2 className="size-4 mr-2 animate-spin" />
                      ) : (
                        <Send className="size-4 mr-2" />
                      )}
                      Enviar Mensagem
                    </Button>
                  </div>
                </div>

                <div>
                  <h3 className="text-sm font-medium mb-3">Histórico</h3>
                  <ScrollArea className="h-[200px]">
                    {data360?.messages?.length > 0 ? (
                      <div className="space-y-3">
                        {data360.messages.map((msg: any) => (
                          <div key={msg.id} className="p-3 rounded-lg border bg-card text-sm">
                            <div className="flex items-center justify-between mb-1">
                              <Badge variant="outline" className="text-xs gap-1">
                                <Mail className="size-3" />
                                {msg.channel}
                              </Badge>
                              <span className="text-xs text-muted-foreground">
                                {format(new Date(msg.createdAt), "d MMM, HH:mm", { locale: ptBR })}
                              </span>
                            </div>
                            {msg.subject && <p className="font-medium">{msg.subject}</p>}
                            <p className="text-muted-foreground line-clamp-2">{msg.content}</p>
                          </div>
                        ))}
                      </div>
                    ) : (
                      <div className="h-full flex items-center justify-center text-muted-foreground">
                        Nenhuma mensagem enviada
                      </div>
                    )}
                  </ScrollArea>
                </div>
              </div>
            </TactileCard>
          </>
        )}
      </div>

      <Dialog open={documentModalOpen} onOpenChange={setDocumentModalOpen}>
        <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>
              {editingDocument ? 'Editar Documento' : 'Adicionar Documento'}
            </DialogTitle>
          </DialogHeader>
          
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="doc-name">Nome *</Label>
              <Input
                id="doc-name"
                value={documentForm.name}
                onChange={(e) => setDocumentForm({ ...documentForm, name: e.target.value })}
                placeholder="Nome do documento"
                data-testid="input-document-name"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="doc-description">Descrição</Label>
              <Textarea
                id="doc-description"
                value={documentForm.description}
                onChange={(e) => setDocumentForm({ ...documentForm, description: e.target.value })}
                placeholder="Descrição do documento"
                rows={2}
                data-testid="input-document-description"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="doc-type">Tipo de Documento</Label>
                <Select
                  value={documentForm.documentType}
                  onValueChange={(value) => setDocumentForm({ ...documentForm, documentType: value })}
                >
                  <SelectTrigger data-testid="select-document-type">
                    <SelectValue placeholder="Selecione o tipo" />
                  </SelectTrigger>
                  <SelectContent>
                    {DOCUMENT_TYPES.map((type) => (
                      <SelectItem key={type.value} value={type.value}>
                        {type.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="doc-number">Número do Documento</Label>
                <Input
                  id="doc-number"
                  value={documentForm.documentNumber}
                  onChange={(e) => setDocumentForm({ ...documentForm, documentNumber: e.target.value })}
                  placeholder="Ex: 123456"
                  data-testid="input-document-number"
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="doc-issue-date">Data de Emissão</Label>
                <Input
                  id="doc-issue-date"
                  type="date"
                  value={documentForm.issueDate}
                  onChange={(e) => setDocumentForm({ ...documentForm, issueDate: e.target.value })}
                  data-testid="input-document-issue-date"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="doc-expiration-date">Data de Vencimento</Label>
                <Input
                  id="doc-expiration-date"
                  type="date"
                  value={documentForm.expirationDate}
                  onChange={(e) => setDocumentForm({ ...documentForm, expirationDate: e.target.value })}
                  data-testid="input-document-expiration-date"
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="doc-alert-days">Alertar dias antes do vencimento</Label>
              <Input
                id="doc-alert-days"
                type="number"
                min={1}
                max={365}
                value={documentForm.alertDaysBefore}
                onChange={(e) => setDocumentForm({ ...documentForm, alertDaysBefore: parseInt(e.target.value) || 30 })}
                data-testid="input-document-alert-days"
              />
            </div>

            <div className="space-y-2">
              <Label>Arquivo</Label>
              {uploadedFile ? (
                <div className="flex items-center gap-2 p-3 rounded-lg border bg-muted/50">
                  <FileUp className="size-5 text-muted-foreground" />
                  <span className="flex-1 truncate text-sm">{uploadedFile.name}</span>
                  <Button
                    variant="ghost"
                    size="icon"
                    className="shrink-0 h-8 w-8"
                    onClick={() => setUploadedFile(null)}
                  >
                    <X className="size-4" />
                  </Button>
                </div>
              ) : (
                <ObjectUploader
                  maxNumberOfFiles={1}
                  maxFileSize={50 * 1024 * 1024}
                  onGetUploadParameters={getUploadParameters}
                  onComplete={handleUploadComplete}
                  buttonClassName="w-full"
                >
                  <FileUp className="size-4 mr-2" />
                  Selecionar Arquivo
                </ObjectUploader>
              )}
            </div>

            <div className="flex justify-end gap-2 pt-4">
              <Button variant="outline" onClick={resetDocumentForm}>
                Cancelar
              </Button>
              <Button 
                onClick={handleSubmitDocument}
                disabled={createDocumentMutation.isPending || updateDocumentMutation.isPending}
                data-testid="button-save-document"
              >
                {(createDocumentMutation.isPending || updateDocumentMutation.isPending) && (
                  <Loader2 className="size-4 mr-2 animate-spin" />
                )}
                {editingDocument ? 'Salvar' : 'Adicionar'}
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Excluir documento?</AlertDialogTitle>
            <AlertDialogDescription>
              Tem certeza que deseja excluir o documento "{documentToDelete?.name}"? 
              Esta ação não pode ser desfeita.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancelar</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => documentToDelete && deleteDocumentMutation.mutate(documentToDelete.id)}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              {deleteDocumentMutation.isPending && <Loader2 className="size-4 mr-2 animate-spin" />}
              Excluir
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </AppShell>
  );
}
