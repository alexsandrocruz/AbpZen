import { useState, useEffect, useMemo } from "react";
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import { useParams, useLocation, useRoute } from "wouter";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
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
import { Loader2, Plus, Trash2, Package, Percent, ArrowLeft, Save } from "lucide-react";
import { ProposalChatbot } from "@/components/ai/proposal-chatbot";
import { ClientCombobox } from "@/components/ui/client-combobox";
import { ProductCombobox } from "@/components/ui/product-combobox";
import { CustomFieldsSection } from "@/components/custom-fields-section";

interface ProposalItem {
  productId?: string;
  description: string;
  quantity: number;
  price: string;
  discount: string;
  discountType: "PERCENT" | "FIXED";
}

export default function ProposalFormPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [, navigate] = useLocation();
  const params = useParams<{ slug: string }>();
  const [isEditRoute, editParams] = useRoute("/:slug/proposals/:id/edit");
  const proposalId = isEditRoute ? editParams?.id : undefined;
  const isEditing = !!proposalId;

  const { data: proposal, isLoading: proposalLoading } = useQuery({
    queryKey: ['proposal', currentWorkspace?.id, proposalId],
    queryFn: () => api.getProposal(currentWorkspace!.id, proposalId!),
    enabled: !!currentWorkspace && !!proposalId,
  });

  const { data: clients = [] } = useQuery({
    queryKey: ['clients', currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: products = [] } = useQuery({
    queryKey: ['products', currentWorkspace?.id],
    queryFn: () => api.getProducts(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: existingItems = [] } = useQuery({
    queryKey: ['proposalItems', currentWorkspace?.id, proposalId],
    queryFn: () => api.getProposalItems(currentWorkspace!.id, proposalId!),
    enabled: !!currentWorkspace && !!proposalId,
  });

  const [formData, setFormData] = useState({
    title: "",
    clientId: "",
    content: "",
    contractText: "",
    status: "DRAFT",
    validUntil: "",
    globalDiscount: "0",
    globalDiscountType: "PERCENT" as "PERCENT" | "FIXED",
  });

  const [items, setItems] = useState<ProposalItem[]>([
    { description: "", quantity: 1, price: "", discount: "0", discountType: "PERCENT" }
  ]);

  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});

  const { data: customFieldDefinitions = [] } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, "PROPOSAL"],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, "PROPOSAL"),
    enabled: !!currentWorkspace,
  });

  const { data: existingCustomFieldValues = [] } = useQuery({
    queryKey: ['customFieldValues', currentWorkspace?.id, 'PROPOSAL', proposalId],
    queryFn: () => api.getCustomFieldValues(currentWorkspace!.id, "PROPOSAL", proposalId!),
    enabled: !!currentWorkspace && !!proposalId,
  });

  useEffect(() => {
    if (existingCustomFieldValues.length > 0 && proposalId) {
      const values: Record<string, any> = {};
      existingCustomFieldValues.forEach((cfv: any) => {
        const def = customFieldDefinitions.find((d: any) => d.id === cfv.definitionId);
        if (def) {
          values[def.fieldKey] = cfv.value;
        }
      });
      setCustomFieldValues(values);
    }
  }, [existingCustomFieldValues, proposalId, customFieldDefinitions]);

  useEffect(() => {
    if (proposal) {
      setFormData({
        title: proposal.title || "",
        clientId: proposal.clientId || "",
        content: proposal.content || "",
        contractText: proposal.contractText || "",
        status: proposal.status || "DRAFT",
        validUntil: proposal.validUntil ? new Date(proposal.validUntil).toISOString().split('T')[0] : "",
        globalDiscount: proposal.globalDiscount || "0",
        globalDiscountType: proposal.globalDiscountType || "PERCENT",
      });
    } else if (!proposalId) {
      setFormData({ 
        title: "", clientId: "", content: "", contractText: "", status: "DRAFT", 
        validUntil: "", globalDiscount: "0", globalDiscountType: "PERCENT" 
      });
      setItems([{ description: "", quantity: 1, price: "", discount: "0", discountType: "PERCENT" }]);
      setCustomFieldValues({});
    }
  }, [proposal, proposalId]);

  useEffect(() => {
    if (existingItems.length > 0 && proposalId) {
      const loadedItems = existingItems.map((item: any) => ({
        productId: item.productId || undefined,
        description: item.description || "",
        quantity: item.quantity || 1,
        price: item.price || "0",
        discount: item.discount || "0",
        discountType: (item.discountType as "PERCENT" | "FIXED") || "PERCENT",
      }));
      setItems(loadedItems);
    }
  }, [existingItems, proposalId]);

  const saveCustomFields = async (entityId: string) => {
    if (customFieldDefinitions.length === 0) return;
    
    const valuesToSave = customFieldDefinitions.map((def: any) => {
      const rawValue = customFieldValues[def.fieldKey];
      let value: any = null;

      if (def.fieldType === "CHECKBOX") {
        value = rawValue === true || rawValue === false ? rawValue : false;
      } else if (def.fieldType === "NUMBER" || def.fieldType === "CURRENCY") {
        const num = parseFloat(rawValue);
        value = !isNaN(num) ? num : null;
      } else if (def.fieldType === "DATE") {
        value = rawValue || null;
      } else if (def.fieldType === "MULTISELECT") {
        value = Array.isArray(rawValue) ? rawValue : [];
      } else {
        value = rawValue !== undefined && rawValue !== null && rawValue !== "" ? String(rawValue) : null;
      }

      return { definitionId: def.id, value };
    });

    await api.saveCustomFieldValues(currentWorkspace!.id, "PROPOSAL", entityId, valuesToSave);
  };

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createProposal(currentWorkspace!.id, data),
    onSuccess: async (newProposal: any) => {
      await saveCustomFields(newProposal.id);
      queryClient.invalidateQueries({ queryKey: ['proposals', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Proposta criada com sucesso!" });
      navigate(`/${params.slug}/proposals`);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateProposal(currentWorkspace!.id, proposalId!, data),
    onSuccess: async () => {
      await saveCustomFields(proposalId!);
      queryClient.invalidateQueries({ queryKey: ['proposals', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Proposta atualizada com sucesso!" });
      navigate(`/${params.slug}/proposals`);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const validItems = items.filter(item => item.description && item.price);
    const dataToSend = {
      ...formData,
      validUntil: formData.validUntil || null,
      items: validItems,
    };
    if (isEditing) {
      updateMutation.mutate(dataToSend);
    } else {
      createMutation.mutate(dataToSend);
    }
  };

  const addItem = () => {
    setItems([...items, { description: "", quantity: 1, price: "", discount: "0", discountType: "PERCENT" }]);
  };

  const addProductItem = (productId: string) => {
    const product = products.find((p: any) => p.id === productId);
    if (product) {
      setItems([...items, { 
        productId: product.id,
        description: product.name, 
        quantity: 1, 
        price: product.price,
        discount: "0",
        discountType: "PERCENT"
      }]);
    }
  };

  const removeItem = (index: number) => {
    setItems(items.filter((_, i) => i !== index));
  };

  const updateItem = (index: number, field: keyof ProposalItem, value: any) => {
    const newItems = [...items];
    newItems[index] = { ...newItems[index], [field]: value };
    setItems(newItems);
  };

  const updateItemMultiple = (index: number, updates: Partial<ProposalItem>) => {
    const newItems = [...items];
    newItems[index] = { ...newItems[index], ...updates };
    setItems(newItems);
  };

  const calculateItemTotal = (item: ProposalItem) => {
    const subtotal = item.quantity * parseFloat(item.price || "0");
    const discountValue = parseFloat(item.discount || "0");
    let total: number;
    if (item.discountType === "PERCENT") {
      const clampedDiscount = Math.min(Math.max(discountValue, 0), 100);
      total = subtotal * (1 - clampedDiscount / 100);
    } else {
      total = subtotal - Math.min(discountValue, subtotal);
    }
    return Math.max(total, 0);
  };

  const subtotal = useMemo(() => {
    return items.reduce((total, item) => total + calculateItemTotal(item), 0);
  }, [items]);

  const globalDiscountValue = useMemo(() => {
    const discount = parseFloat(formData.globalDiscount || "0");
    if (formData.globalDiscountType === "PERCENT") {
      const clampedDiscount = Math.min(Math.max(discount, 0), 100);
      return subtotal * (clampedDiscount / 100);
    }
    return Math.min(Math.max(discount, 0), subtotal);
  }, [subtotal, formData.globalDiscount, formData.globalDiscountType]);

  const total = Math.max(subtotal - globalDiscountValue, 0);

  const productCategories = useMemo(() => {
    const categories = new Map<string, any[]>();
    products.forEach((p: any) => {
      const cat = p.category || "Sem categoria";
      if (!categories.has(cat)) {
        categories.set(cat, []);
      }
      categories.get(cat)!.push(p);
    });
    return categories;
  }, [products]);

  const isLoading = createMutation.isPending || updateMutation.isPending;

  if (proposalLoading && isEditing) {
    return (
      <AppShell>
        <div className="flex items-center justify-center h-64">
          <Loader2 className="size-8 animate-spin text-muted-foreground" />
        </div>
      </AppShell>
    );
  }

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button 
              variant="ghost" 
              size="icon"
              onClick={() => navigate(`/${params.slug}/proposals`)}
              data-testid="button-back"
            >
              <ArrowLeft className="size-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Editar Proposta" : "Nova Proposta"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Altere os dados da proposta" : "Crie uma nova proposta comercial"}
              </p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <ProposalChatbot 
              clientInfo={formData.clientId ? {
                name: clients.find((c: any) => c.id === formData.clientId)?.companyName,
                company: clients.find((c: any) => c.id === formData.clientId)?.companyName,
                email: clients.find((c: any) => c.id === formData.clientId)?.email,
              } : undefined}
              onInsertContent={(content) => setFormData({ ...formData, content: formData.content + "\n\n" + content })}
            />
          </div>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            <div className="lg:col-span-2 space-y-6">
              <TactileCard className="p-6">
                <h2 className="text-lg font-semibold mb-4">Informações Básicas</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="title">Título</Label>
                    <Input
                      id="title"
                      value={formData.title}
                      onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                      placeholder="Título da proposta"
                      required
                      data-testid="input-proposal-title"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="clientId">Cliente</Label>
                    <ClientCombobox
                      clients={clients}
                      value={formData.clientId}
                      onChange={(clientId) => setFormData({ ...formData, clientId })}
                      workspaceId={currentWorkspace!.id}
                      placeholder="Buscar ou criar cliente..."
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="status">Status</Label>
                    <Select
                      key={formData.status}
                      value={formData.status || "DRAFT"}
                      onValueChange={(value) => setFormData({ ...formData, status: value })}
                    >
                      <SelectTrigger data-testid="select-proposal-status">
                        <SelectValue placeholder="Selecione o status" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="DRAFT">Rascunho</SelectItem>
                        <SelectItem value="SENT">Enviada</SelectItem>
                        <SelectItem value="ACTIVE">Aceita</SelectItem>
                        <SelectItem value="ARCHIVED">Recusada</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="validUntil">Validade</Label>
                    <Input
                      id="validUntil"
                      type="date"
                      value={formData.validUntil}
                      onChange={(e) => setFormData({ ...formData, validUntil: e.target.value })}
                      data-testid="input-proposal-validity"
                    />
                  </div>
                </div>
                <div className="space-y-2 mt-4">
                  <Label htmlFor="content">Descrição da Proposta</Label>
                  <Textarea
                    id="content"
                    value={formData.content}
                    onChange={(e) => setFormData({ ...formData, content: e.target.value })}
                    placeholder="Descreva os serviços e entregas..."
                    rows={4}
                    required
                    data-testid="input-proposal-content"
                  />
                </div>
              </TactileCard>

              <TactileCard className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <h2 className="text-lg font-semibold">Itens do Orçamento</h2>
                  <div className="flex gap-2">
                    {products.length > 0 && (
                      <Select onValueChange={addProductItem}>
                        <SelectTrigger className="w-[180px]" data-testid="select-add-product">
                          <Package className="size-4 mr-2" />
                          <SelectValue placeholder="Do catálogo..." />
                        </SelectTrigger>
                        <SelectContent>
                          {Array.from(productCategories.entries()).map(([category, prods]) => (
                            <div key={category}>
                              <div className="px-2 py-1.5 text-xs font-semibold text-muted-foreground">{category}</div>
                              {prods.map((product: any) => (
                                <SelectItem key={product.id} value={product.id}>
                                  {product.name} - R$ {parseFloat(product.price).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                                </SelectItem>
                              ))}
                            </div>
                          ))}
                        </SelectContent>
                      </Select>
                    )}
                    <Button type="button" variant="outline" size="sm" onClick={addItem} data-testid="button-add-item">
                      <Plus className="size-4 mr-1" /> Item Manual
                    </Button>
                  </div>
                </div>

                <div className="space-y-3">
                  {items.map((item, index) => (
                    <div key={index} className="p-4 border rounded-lg space-y-3 bg-muted/30">
                      <div className="flex gap-3 items-start">
                        <div className="flex-1 space-y-3">
                          <ProductCombobox
                            products={products}
                            value={item.productId}
                            onChange={(productId, product) => {
                              if (product) {
                                updateItemMultiple(index, {
                                  productId,
                                  description: product.name,
                                  price: product.price,
                                });
                              } else {
                                updateItem(index, 'productId', undefined);
                              }
                            }}
                            workspaceId={currentWorkspace?.id || ""}
                            placeholder="Selecione ou crie um produto..."
                          />
                          <Input
                            placeholder="Descrição do item"
                            value={item.description}
                            onChange={(e) => updateItem(index, 'description', e.target.value)}
                            data-testid={`input-proposal-item-description-${index}`}
                          />
                        </div>
                        {items.length > 1 && (
                          <Button type="button" variant="ghost" size="icon" onClick={() => removeItem(index)} className="mt-1">
                            <Trash2 className="size-4 text-destructive" />
                          </Button>
                        )}
                      </div>
                      <div className="grid grid-cols-2 md:grid-cols-5 gap-3">
                        <div>
                          <Label className="text-xs text-muted-foreground">Qtd</Label>
                          <Input
                            type="number"
                            min="1"
                            value={item.quantity}
                            onChange={(e) => updateItem(index, 'quantity', parseInt(e.target.value) || 1)}
                            data-testid={`input-proposal-item-quantity-${index}`}
                          />
                        </div>
                        <div>
                          <Label className="text-xs text-muted-foreground">Preço Unit.</Label>
                          <Input
                            type="number"
                            step="0.01"
                            value={item.price}
                            onChange={(e) => updateItem(index, 'price', e.target.value)}
                            data-testid={`input-proposal-item-price-${index}`}
                          />
                        </div>
                        <div>
                          <Label className="text-xs text-muted-foreground">Desconto</Label>
                          <Input
                            type="number"
                            step="0.01"
                            min="0"
                            value={item.discount}
                            onChange={(e) => updateItem(index, 'discount', e.target.value)}
                            data-testid={`input-proposal-item-discount-${index}`}
                          />
                        </div>
                        <div>
                          <Label className="text-xs text-muted-foreground">Tipo</Label>
                          <Select
                            value={item.discountType}
                            onValueChange={(value) => updateItem(index, 'discountType', value)}
                          >
                            <SelectTrigger data-testid={`select-proposal-item-discount-type-${index}`}>
                              <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value="PERCENT">%</SelectItem>
                              <SelectItem value="FIXED">R$</SelectItem>
                            </SelectContent>
                          </Select>
                        </div>
                        <div>
                          <Label className="text-xs text-muted-foreground">Subtotal</Label>
                          <div className="h-9 flex items-center px-3 border rounded-md bg-background font-medium">
                            R$ {calculateItemTotal(item).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </TactileCard>

              <TactileCard className="p-6">
                <h2 className="text-lg font-semibold mb-4">Termos e Condições</h2>
                <Textarea
                  id="contractText"
                  value={formData.contractText}
                  onChange={(e) => setFormData({ ...formData, contractText: e.target.value })}
                  placeholder="Termos e condições do contrato..."
                  rows={4}
                  data-testid="input-proposal-contract"
                />
              </TactileCard>

              {customFieldDefinitions.length > 0 && (
                <TactileCard className="p-6">
                  <h2 className="text-lg font-semibold mb-4">Campos Personalizados</h2>
                  <CustomFieldsSection
                    entityType="PROPOSAL"
                    entityId={proposalId}
                    values={customFieldValues}
                    onChange={setCustomFieldValues}
                  />
                </TactileCard>
              )}
            </div>

            <div className="space-y-6">
              <TactileCard className="p-6 sticky top-6">
                <h2 className="text-lg font-semibold mb-4">Resumo</h2>
                <div className="space-y-3">
                  <div className="flex justify-between items-center text-sm">
                    <span className="text-muted-foreground">Subtotal:</span>
                    <span>R$ {subtotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                  </div>
                  <div className="flex flex-col gap-2">
                    <span className="text-sm text-muted-foreground flex items-center gap-1">
                      <Percent className="size-3" /> Desconto Global:
                    </span>
                    <div className="flex gap-2">
                      <Input
                        type="number"
                        step="0.01"
                        min="0"
                        className="flex-1"
                        value={formData.globalDiscount}
                        onChange={(e) => setFormData({ ...formData, globalDiscount: e.target.value })}
                        data-testid="input-global-discount"
                      />
                      <Select
                        value={formData.globalDiscountType}
                        onValueChange={(value: "PERCENT" | "FIXED") => setFormData({ ...formData, globalDiscountType: value })}
                      >
                        <SelectTrigger className="w-20" data-testid="select-global-discount-type">
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="PERCENT">%</SelectItem>
                          <SelectItem value="FIXED">R$</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>
                  {globalDiscountValue > 0 && (
                    <div className="flex justify-between items-center text-sm text-red-600">
                      <span>Desconto:</span>
                      <span>- R$ {globalDiscountValue.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                    </div>
                  )}
                  <div className="border-t pt-3 flex justify-between items-center text-xl font-bold">
                    <span>Total:</span>
                    <span className="text-primary">R$ {total.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</span>
                  </div>
                </div>

                <div className="mt-6 flex flex-col gap-2">
                  <Button type="submit" disabled={isLoading || !formData.clientId} className="w-full" data-testid="button-save-proposal">
                    {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
                    <Save className="mr-2 size-4" />
                    {isEditing ? "Salvar Alterações" : "Criar Proposta"}
                  </Button>
                  <Button 
                    type="button" 
                    variant="outline" 
                    className="w-full"
                    onClick={() => navigate(`/${params.slug}/proposals`)}
                  >
                    Cancelar
                  </Button>
                </div>
              </TactileCard>
            </div>
          </div>
        </form>
      </div>
    </AppShell>
  );
}
