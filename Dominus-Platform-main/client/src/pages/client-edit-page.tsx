import { useState, useEffect } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams, useLocation } from "wouter";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import { AppShell } from "@/components/layout/shell";
import { TactileCard } from "@/components/ui/tactile-card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Skeleton } from "@/components/ui/skeleton";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ArrowLeft, Save, Loader2, Building2, User, MapPin, CreditCard, FileText } from "lucide-react";
import { CustomFieldsSection } from "@/components/custom-fields-section";

export default function ClientEditPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const [, navigate] = useLocation();
  const params = useParams<{ slug: string; id: string }>();
  const clientId = params.id;

  const { data: client, isLoading } = useQuery({
    queryKey: ['client', currentWorkspace?.id, clientId],
    queryFn: () => api.getClient(currentWorkspace!.id, clientId!),
    enabled: !!currentWorkspace && !!clientId,
  });

  const [formData, setFormData] = useState({
    name: "",
    email: "",
    phone: "",
    clientType: "COMPANY" as "COMPANY" | "PERSON",
    companyName: "",
    tradeName: "",
    cnpj: "",
    stateRegistration: "",
    municipalRegistration: "",
    cpf: "",
    rg: "",
    birthDate: "",
    address: "",
    addressNumber: "",
    complement: "",
    neighborhood: "",
    city: "",
    state: "",
    zipCode: "",
    bankName: "",
    bankAgency: "",
    bankAccount: "",
    pixKey: "",
    paymentTermDays: "30",
    notes: "",
    status: "ACTIVE",
  });

  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});

  const { data: customFieldDefinitions = [] } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, "CLIENT"],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, "CLIENT"),
    enabled: !!currentWorkspace,
  });

  const { data: existingCustomFieldValues = [] } = useQuery({
    queryKey: ['customFieldValues', currentWorkspace?.id, 'CLIENT', clientId],
    queryFn: () => api.getCustomFieldValues(currentWorkspace!.id, "CLIENT", clientId!),
    enabled: !!currentWorkspace && !!clientId,
  });

  useEffect(() => {
    if (existingCustomFieldValues.length > 0 && clientId) {
      const values: Record<string, any> = {};
      existingCustomFieldValues.forEach((cfv: any) => {
        const def = customFieldDefinitions.find((d: any) => d.id === cfv.definitionId);
        if (def) {
          values[def.fieldKey] = cfv.value;
        }
      });
      setCustomFieldValues(values);
    }
  }, [existingCustomFieldValues, clientId, customFieldDefinitions]);

  useEffect(() => {
    if (client) {
      setFormData({
        name: client.name || "",
        email: client.email || "",
        phone: client.phone || "",
        clientType: client.clientType || "COMPANY",
        companyName: client.companyName || "",
        tradeName: client.tradeName || "",
        cnpj: client.cnpj || "",
        stateRegistration: client.stateRegistration || "",
        municipalRegistration: client.municipalRegistration || "",
        cpf: client.cpf || "",
        rg: client.rg || "",
        birthDate: client.birthDate ? new Date(client.birthDate).toISOString().split('T')[0] : "",
        address: client.address || "",
        addressNumber: client.addressNumber || "",
        complement: client.complement || "",
        neighborhood: client.neighborhood || "",
        city: client.city || "",
        state: client.state || "",
        zipCode: client.zipCode || "",
        bankName: client.bankName || "",
        bankAgency: client.bankAgency || "",
        bankAccount: client.bankAccount || "",
        pixKey: client.pixKey || "",
        paymentTermDays: client.paymentTermDays?.toString() || "30",
        notes: client.notes || "",
        status: client.status || "ACTIVE",
      });
    }
  }, [client]);

  const updateMutation = useMutation({
    mutationFn: async (data: any) => {
      const result = await api.updateClient(currentWorkspace!.id, clientId!, data);
      
      if (customFieldDefinitions.length > 0 && Object.keys(customFieldValues).length > 0) {
        const cfPayload = customFieldDefinitions.map((def: any) => ({
          definitionId: def.id,
          value: customFieldValues[def.fieldKey] ?? null,
        }));
        await api.saveCustomFieldValues(currentWorkspace!.id, "CLIENT", clientId!, cfPayload);
      }
      
      return result;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['client', currentWorkspace?.id, clientId] });
      queryClient.invalidateQueries({ queryKey: ['client360', currentWorkspace?.id, clientId] });
      queryClient.invalidateQueries({ queryKey: ['clients', currentWorkspace?.id] });
      toast({ title: "Sucesso", description: "Cliente atualizado com sucesso!" });
      navigate(`/${currentWorkspace?.slug}/clients/${clientId}`);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.name.trim() || !formData.email.trim()) {
      toast({ title: "Erro", description: "Nome e email são obrigatórios", variant: "destructive" });
      return;
    }

    updateMutation.mutate({
      ...formData,
      paymentTermDays: parseInt(formData.paymentTermDays) || 30,
      birthDate: formData.birthDate || null,
    });
  };

  if (!currentWorkspace?.slug) {
    return null;
  }

  return (
    <AppShell>
      <div className="p-6 max-w-4xl mx-auto space-y-6">
        <div className="flex items-center gap-4">
          <Button 
            variant="ghost" 
            size="icon"
            onClick={() => navigate(`/${currentWorkspace.slug}/clients/${clientId}`)}
            data-testid="button-back"
          >
            <ArrowLeft className="size-5" />
          </Button>
          <div className="flex-1">
            <h1 className="text-2xl font-bold">
              {isLoading ? <Skeleton className="h-8 w-48" /> : `Editar: ${client?.name}`}
            </h1>
            <p className="text-muted-foreground">Edite as informações do cliente</p>
          </div>
          <Button 
            onClick={handleSubmit}
            disabled={updateMutation.isPending}
            data-testid="button-save-client"
          >
            {updateMutation.isPending ? (
              <Loader2 className="size-4 mr-2 animate-spin" />
            ) : (
              <Save className="size-4 mr-2" />
            )}
            Salvar Alterações
          </Button>
        </div>

        {isLoading ? (
          <div className="space-y-4">
            <Skeleton className="h-64 rounded-xl" />
            <Skeleton className="h-64 rounded-xl" />
          </div>
        ) : (
          <form onSubmit={handleSubmit}>
            <Tabs defaultValue="basic" className="w-full">
              <TabsList className="mb-4">
                <TabsTrigger value="basic" className="gap-2">
                  {formData.clientType === 'COMPANY' ? <Building2 className="size-4" /> : <User className="size-4" />}
                  Dados Básicos
                </TabsTrigger>
                <TabsTrigger value="address" className="gap-2">
                  <MapPin className="size-4" />
                  Endereço
                </TabsTrigger>
                <TabsTrigger value="financial" className="gap-2">
                  <CreditCard className="size-4" />
                  Financeiro
                </TabsTrigger>
                <TabsTrigger value="additional" className="gap-2">
                  <FileText className="size-4" />
                  Adicional
                </TabsTrigger>
              </TabsList>

              <TabsContent value="basic">
                <TactileCard className="p-6 space-y-4">
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="clientType">Tipo de Cliente *</Label>
                      <Select 
                        value={formData.clientType} 
                        onValueChange={(value: "COMPANY" | "PERSON") => setFormData({ ...formData, clientType: value })}
                      >
                        <SelectTrigger data-testid="select-client-type">
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="COMPANY">Pessoa Jurídica (PJ)</SelectItem>
                          <SelectItem value="PERSON">Pessoa Física (PF)</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="status">Status</Label>
                      <Select 
                        value={formData.status} 
                        onValueChange={(value) => setFormData({ ...formData, status: value })}
                      >
                        <SelectTrigger data-testid="select-status">
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="ACTIVE">Ativo</SelectItem>
                          <SelectItem value="ARCHIVED">Arquivado</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>

                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="name">Nome *</Label>
                      <Input
                        id="name"
                        value={formData.name}
                        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                        placeholder="Nome completo"
                        data-testid="input-name"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="email">Email *</Label>
                      <Input
                        id="email"
                        type="email"
                        value={formData.email}
                        onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        placeholder="email@exemplo.com"
                        data-testid="input-email"
                      />
                    </div>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="phone">Telefone</Label>
                    <Input
                      id="phone"
                      value={formData.phone}
                      onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                      placeholder="(11) 99999-9999"
                      data-testid="input-phone"
                    />
                  </div>

                  {formData.clientType === 'COMPANY' ? (
                    <>
                      <div className="grid gap-4 md:grid-cols-2">
                        <div className="space-y-2">
                          <Label htmlFor="companyName">Razão Social</Label>
                          <Input
                            id="companyName"
                            value={formData.companyName}
                            onChange={(e) => setFormData({ ...formData, companyName: e.target.value })}
                            placeholder="Razão Social da Empresa"
                            data-testid="input-company-name"
                          />
                        </div>

                        <div className="space-y-2">
                          <Label htmlFor="tradeName">Nome Fantasia</Label>
                          <Input
                            id="tradeName"
                            value={formData.tradeName}
                            onChange={(e) => setFormData({ ...formData, tradeName: e.target.value })}
                            placeholder="Nome Fantasia"
                            data-testid="input-trade-name"
                          />
                        </div>
                      </div>

                      <div className="grid gap-4 md:grid-cols-3">
                        <div className="space-y-2">
                          <Label htmlFor="cnpj">CNPJ</Label>
                          <Input
                            id="cnpj"
                            value={formData.cnpj}
                            onChange={(e) => setFormData({ ...formData, cnpj: e.target.value })}
                            placeholder="00.000.000/0000-00"
                            data-testid="input-cnpj"
                          />
                        </div>

                        <div className="space-y-2">
                          <Label htmlFor="stateRegistration">Inscrição Estadual</Label>
                          <Input
                            id="stateRegistration"
                            value={formData.stateRegistration}
                            onChange={(e) => setFormData({ ...formData, stateRegistration: e.target.value })}
                            placeholder="Inscrição Estadual"
                            data-testid="input-state-registration"
                          />
                        </div>

                        <div className="space-y-2">
                          <Label htmlFor="municipalRegistration">Inscrição Municipal</Label>
                          <Input
                            id="municipalRegistration"
                            value={formData.municipalRegistration}
                            onChange={(e) => setFormData({ ...formData, municipalRegistration: e.target.value })}
                            placeholder="Inscrição Municipal"
                            data-testid="input-municipal-registration"
                          />
                        </div>
                      </div>
                    </>
                  ) : (
                    <div className="grid gap-4 md:grid-cols-3">
                      <div className="space-y-2">
                        <Label htmlFor="cpf">CPF</Label>
                        <Input
                          id="cpf"
                          value={formData.cpf}
                          onChange={(e) => setFormData({ ...formData, cpf: e.target.value })}
                          placeholder="000.000.000-00"
                          data-testid="input-cpf"
                        />
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="rg">RG</Label>
                        <Input
                          id="rg"
                          value={formData.rg}
                          onChange={(e) => setFormData({ ...formData, rg: e.target.value })}
                          placeholder="RG"
                          data-testid="input-rg"
                        />
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="birthDate">Data de Nascimento</Label>
                        <Input
                          id="birthDate"
                          type="date"
                          value={formData.birthDate}
                          onChange={(e) => setFormData({ ...formData, birthDate: e.target.value })}
                          data-testid="input-birth-date"
                        />
                      </div>
                    </div>
                  )}
                </TactileCard>
              </TabsContent>

              <TabsContent value="address">
                <TactileCard className="p-6 space-y-4">
                  <div className="grid gap-4 md:grid-cols-3">
                    <div className="space-y-2 md:col-span-2">
                      <Label htmlFor="address">Endereço</Label>
                      <Input
                        id="address"
                        value={formData.address}
                        onChange={(e) => setFormData({ ...formData, address: e.target.value })}
                        placeholder="Rua, Avenida..."
                        data-testid="input-address"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="addressNumber">Número</Label>
                      <Input
                        id="addressNumber"
                        value={formData.addressNumber}
                        onChange={(e) => setFormData({ ...formData, addressNumber: e.target.value })}
                        placeholder="123"
                        data-testid="input-address-number"
                      />
                    </div>
                  </div>

                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="complement">Complemento</Label>
                      <Input
                        id="complement"
                        value={formData.complement}
                        onChange={(e) => setFormData({ ...formData, complement: e.target.value })}
                        placeholder="Apto, Sala..."
                        data-testid="input-complement"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="neighborhood">Bairro</Label>
                      <Input
                        id="neighborhood"
                        value={formData.neighborhood}
                        onChange={(e) => setFormData({ ...formData, neighborhood: e.target.value })}
                        placeholder="Bairro"
                        data-testid="input-neighborhood"
                      />
                    </div>
                  </div>

                  <div className="grid gap-4 md:grid-cols-3">
                    <div className="space-y-2">
                      <Label htmlFor="city">Cidade</Label>
                      <Input
                        id="city"
                        value={formData.city}
                        onChange={(e) => setFormData({ ...formData, city: e.target.value })}
                        placeholder="Cidade"
                        data-testid="input-city"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="state">Estado</Label>
                      <Input
                        id="state"
                        value={formData.state}
                        onChange={(e) => setFormData({ ...formData, state: e.target.value })}
                        placeholder="SP"
                        data-testid="input-state"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="zipCode">CEP</Label>
                      <Input
                        id="zipCode"
                        value={formData.zipCode}
                        onChange={(e) => setFormData({ ...formData, zipCode: e.target.value })}
                        placeholder="00000-000"
                        data-testid="input-zip-code"
                      />
                    </div>
                  </div>
                </TactileCard>
              </TabsContent>

              <TabsContent value="financial">
                <TactileCard className="p-6 space-y-4">
                  <div className="grid gap-4 md:grid-cols-2">
                    <div className="space-y-2">
                      <Label htmlFor="bankName">Banco</Label>
                      <Input
                        id="bankName"
                        value={formData.bankName}
                        onChange={(e) => setFormData({ ...formData, bankName: e.target.value })}
                        placeholder="Nome do Banco"
                        data-testid="input-bank-name"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="pixKey">Chave PIX</Label>
                      <Input
                        id="pixKey"
                        value={formData.pixKey}
                        onChange={(e) => setFormData({ ...formData, pixKey: e.target.value })}
                        placeholder="Chave PIX"
                        data-testid="input-pix-key"
                      />
                    </div>
                  </div>

                  <div className="grid gap-4 md:grid-cols-3">
                    <div className="space-y-2">
                      <Label htmlFor="bankAgency">Agência</Label>
                      <Input
                        id="bankAgency"
                        value={formData.bankAgency}
                        onChange={(e) => setFormData({ ...formData, bankAgency: e.target.value })}
                        placeholder="0001"
                        data-testid="input-bank-agency"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="bankAccount">Conta</Label>
                      <Input
                        id="bankAccount"
                        value={formData.bankAccount}
                        onChange={(e) => setFormData({ ...formData, bankAccount: e.target.value })}
                        placeholder="12345-6"
                        data-testid="input-bank-account"
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="paymentTermDays">Prazo de Pagamento (dias)</Label>
                      <Input
                        id="paymentTermDays"
                        type="number"
                        value={formData.paymentTermDays}
                        onChange={(e) => setFormData({ ...formData, paymentTermDays: e.target.value })}
                        placeholder="30"
                        data-testid="input-payment-term"
                      />
                    </div>
                  </div>
                </TactileCard>
              </TabsContent>

              <TabsContent value="additional">
                <TactileCard className="p-6 space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="notes">Observações</Label>
                    <Textarea
                      id="notes"
                      value={formData.notes}
                      onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
                      placeholder="Observações sobre o cliente..."
                      rows={4}
                      data-testid="input-notes"
                    />
                  </div>

                  {customFieldDefinitions.length > 0 && (
                    <CustomFieldsSection
                      entityType="CLIENT"
                      entityId={clientId}
                      values={customFieldValues}
                      onChange={setCustomFieldValues}
                    />
                  )}
                </TactileCard>
              </TabsContent>
            </Tabs>
          </form>
        )}
      </div>
    </AppShell>
  );
}
