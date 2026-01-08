import { useState, useEffect } from "react";
import { useMutation, useQueryClient, useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Loader2, Building2, User, MapPin, Wallet, Settings2 } from "lucide-react";
import { ScrollArea } from "@/components/ui/scroll-area";
import { CustomFieldsSection } from "@/components/custom-fields-section";

interface ClientModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  client?: any;
}

const BRAZILIAN_STATES = [
  "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
  "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
  "RS", "RO", "RR", "SC", "SP", "SE", "TO"
];

function formatCPF(value: string): string {
  const digits = value.replace(/\D/g, '').slice(0, 11);
  return digits
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
}

function formatCNPJ(value: string): string {
  const digits = value.replace(/\D/g, '').slice(0, 14);
  return digits
    .replace(/(\d{2})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1/$2')
    .replace(/(\d{4})(\d{1,2})$/, '$1-$2');
}

function formatCEP(value: string): string {
  const digits = value.replace(/\D/g, '').slice(0, 8);
  return digits.replace(/(\d{5})(\d)/, '$1-$2');
}

function formatPhone(value: string): string {
  const digits = value.replace(/\D/g, '').slice(0, 11);
  if (digits.length <= 10) {
    return digits
      .replace(/(\d{2})(\d)/, '($1) $2')
      .replace(/(\d{4})(\d)/, '$1-$2');
  }
  return digits
    .replace(/(\d{2})(\d)/, '($1) $2')
    .replace(/(\d{5})(\d)/, '$1-$2');
}

export function ClientModal({ open, onOpenChange, client }: ClientModalProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const isEditing = !!client;

  const [formData, setFormData] = useState({
    name: "",
    email: "",
    phone: "",
    clientType: "COMPANY" as "PERSON" | "COMPANY",
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
    paymentTermDays: 30,
    status: "ACTIVE",
  });

  const [customFieldValues, setCustomFieldValues] = useState<Record<string, any>>({});

  const { data: customFieldDefinitions = [] } = useQuery({
    queryKey: ["customFieldDefinitions", currentWorkspace?.id, "CLIENT"],
    queryFn: () => api.getCustomFields(currentWorkspace!.id, "CLIENT"),
    enabled: !!currentWorkspace,
  });

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
        paymentTermDays: client.paymentTermDays || 30,
        status: client.status || "ACTIVE",
      });
      setCustomFieldValues({});
    } else {
      setFormData({
        name: "",
        email: "",
        phone: "",
        clientType: "COMPANY",
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
        paymentTermDays: 30,
        status: "ACTIVE",
      });
      setCustomFieldValues({});
    }
  }, [client, open]);

  const saveCustomFields = async (clientId: string) => {
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

    await api.saveCustomFieldValues(currentWorkspace!.id, "CLIENT", clientId, valuesToSave);
  };

  const createMutation = useMutation({
    mutationFn: (data: any) => api.createClient(currentWorkspace!.id, data),
    onSuccess: async (newClient: any) => {
      await saveCustomFields(newClient.id);
      queryClient.invalidateQueries({ queryKey: ['clients-paginated'] });
      toast({ title: "Sucesso", description: "Cliente criado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const updateMutation = useMutation({
    mutationFn: (data: any) => api.updateClient(currentWorkspace!.id, client.id, data),
    onSuccess: async () => {
      await saveCustomFields(client.id);
      queryClient.invalidateQueries({ queryKey: ['clients-paginated'] });
      toast({ title: "Sucesso", description: "Cliente atualizado com sucesso!" });
      onOpenChange(false);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const dataToSend = {
      ...formData,
      avatar: `https://api.dicebear.com/7.x/initials/svg?seed=${encodeURIComponent(formData.name)}`,
      birthDate: formData.birthDate || null,
    };
    if (isEditing) {
      updateMutation.mutate(dataToSend);
    } else {
      createMutation.mutate(dataToSend);
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[700px] max-h-[90vh]">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Editar Cliente" : "Novo Cliente"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit}>
          <ScrollArea className="h-[60vh] pr-4">
            <Tabs defaultValue="basic" className="w-full">
              <TabsList className={`grid w-full mb-4 ${customFieldDefinitions.length > 0 ? 'grid-cols-5' : 'grid-cols-4'}`}>
                <TabsTrigger value="basic" className="gap-2 text-xs">
                  <User className="size-3.5" />
                  Dados
                </TabsTrigger>
                <TabsTrigger value="documents" className="gap-2 text-xs">
                  <Building2 className="size-3.5" />
                  Documentos
                </TabsTrigger>
                <TabsTrigger value="address" className="gap-2 text-xs">
                  <MapPin className="size-3.5" />
                  Endereço
                </TabsTrigger>
                <TabsTrigger value="financial" className="gap-2 text-xs">
                  <Wallet className="size-3.5" />
                  Financeiro
                </TabsTrigger>
                {customFieldDefinitions.length > 0 && (
                  <TabsTrigger value="custom" className="gap-2 text-xs">
                    <Settings2 className="size-3.5" />
                    Extras
                  </TabsTrigger>
                )}
              </TabsList>

              <TabsContent value="basic" className="space-y-4">
                <div className="space-y-2">
                  <Label>Tipo de Cliente</Label>
                  <Select
                    value={formData.clientType}
                    onValueChange={(value: "PERSON" | "COMPANY") => setFormData({ ...formData, clientType: value })}
                  >
                    <SelectTrigger data-testid="select-client-type">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="PERSON">Pessoa Física (PF)</SelectItem>
                      <SelectItem value="COMPANY">Pessoa Jurídica (PJ)</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="name">
                    {formData.clientType === "PERSON" ? "Nome Completo" : "Nome do Contato"}
                  </Label>
                  <Input
                    id="name"
                    value={formData.name}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    placeholder={formData.clientType === "PERSON" ? "Nome completo" : "Nome do contato principal"}
                    required
                    data-testid="input-client-name"
                  />
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="email">E-mail</Label>
                    <Input
                      id="email"
                      type="email"
                      value={formData.email}
                      onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                      placeholder="email@exemplo.com"
                      required
                      data-testid="input-client-email"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="phone">Telefone</Label>
                    <Input
                      id="phone"
                      value={formData.phone}
                      onChange={(e) => setFormData({ ...formData, phone: formatPhone(e.target.value) })}
                      placeholder="(11) 99999-9999"
                      data-testid="input-client-phone"
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="status">Status</Label>
                  <Select
                    value={formData.status}
                    onValueChange={(value) => setFormData({ ...formData, status: value })}
                  >
                    <SelectTrigger data-testid="select-client-status">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="ACTIVE">Ativo</SelectItem>
                      <SelectItem value="ARCHIVED">Arquivado</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </TabsContent>

              <TabsContent value="documents" className="space-y-4">
                {formData.clientType === "COMPANY" ? (
                  <>
                    <div className="space-y-2">
                      <Label htmlFor="companyName">Razão Social</Label>
                      <Input
                        id="companyName"
                        value={formData.companyName}
                        onChange={(e) => setFormData({ ...formData, companyName: e.target.value })}
                        placeholder="Razão Social da empresa"
                        data-testid="input-client-company-name"
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="tradeName">Nome Fantasia</Label>
                      <Input
                        id="tradeName"
                        value={formData.tradeName}
                        onChange={(e) => setFormData({ ...formData, tradeName: e.target.value })}
                        placeholder="Nome Fantasia"
                        data-testid="input-client-trade-name"
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="cnpj">CNPJ</Label>
                      <Input
                        id="cnpj"
                        value={formData.cnpj}
                        onChange={(e) => setFormData({ ...formData, cnpj: formatCNPJ(e.target.value) })}
                        placeholder="00.000.000/0000-00"
                        data-testid="input-client-cnpj"
                      />
                    </div>
                    <div className="grid grid-cols-2 gap-4">
                      <div className="space-y-2">
                        <Label htmlFor="stateRegistration">Inscrição Estadual</Label>
                        <Input
                          id="stateRegistration"
                          value={formData.stateRegistration}
                          onChange={(e) => setFormData({ ...formData, stateRegistration: e.target.value })}
                          placeholder="Inscrição Estadual"
                          data-testid="input-client-state-registration"
                        />
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor="municipalRegistration">Inscrição Municipal</Label>
                        <Input
                          id="municipalRegistration"
                          value={formData.municipalRegistration}
                          onChange={(e) => setFormData({ ...formData, municipalRegistration: e.target.value })}
                          placeholder="Inscrição Municipal"
                          data-testid="input-client-municipal-registration"
                        />
                      </div>
                    </div>
                  </>
                ) : (
                  <>
                    <div className="space-y-2">
                      <Label htmlFor="cpf">CPF</Label>
                      <Input
                        id="cpf"
                        value={formData.cpf}
                        onChange={(e) => setFormData({ ...formData, cpf: formatCPF(e.target.value) })}
                        placeholder="000.000.000-00"
                        data-testid="input-client-cpf"
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="rg">RG</Label>
                      <Input
                        id="rg"
                        value={formData.rg}
                        onChange={(e) => setFormData({ ...formData, rg: e.target.value })}
                        placeholder="Número do RG"
                        data-testid="input-client-rg"
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="birthDate">Data de Nascimento</Label>
                      <Input
                        id="birthDate"
                        type="date"
                        value={formData.birthDate}
                        onChange={(e) => setFormData({ ...formData, birthDate: e.target.value })}
                        data-testid="input-client-birth-date"
                      />
                    </div>
                  </>
                )}
              </TabsContent>

              <TabsContent value="address" className="space-y-4">
                <div className="grid grid-cols-3 gap-4">
                  <div className="col-span-2 space-y-2">
                    <Label htmlFor="address">Endereço</Label>
                    <Input
                      id="address"
                      value={formData.address}
                      onChange={(e) => setFormData({ ...formData, address: e.target.value })}
                      placeholder="Rua, Avenida, etc."
                      data-testid="input-client-address"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="addressNumber">Número</Label>
                    <Input
                      id="addressNumber"
                      value={formData.addressNumber}
                      onChange={(e) => setFormData({ ...formData, addressNumber: e.target.value })}
                      placeholder="Nº"
                      data-testid="input-client-address-number"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="complement">Complemento</Label>
                    <Input
                      id="complement"
                      value={formData.complement}
                      onChange={(e) => setFormData({ ...formData, complement: e.target.value })}
                      placeholder="Apto, Sala, etc."
                      data-testid="input-client-complement"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="neighborhood">Bairro</Label>
                    <Input
                      id="neighborhood"
                      value={formData.neighborhood}
                      onChange={(e) => setFormData({ ...formData, neighborhood: e.target.value })}
                      placeholder="Bairro"
                      data-testid="input-client-neighborhood"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-3 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="city">Cidade</Label>
                    <Input
                      id="city"
                      value={formData.city}
                      onChange={(e) => setFormData({ ...formData, city: e.target.value })}
                      placeholder="Cidade"
                      data-testid="input-client-city"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="state">Estado</Label>
                    <Select
                      value={formData.state}
                      onValueChange={(value) => setFormData({ ...formData, state: value })}
                    >
                      <SelectTrigger data-testid="select-client-state">
                        <SelectValue placeholder="UF" />
                      </SelectTrigger>
                      <SelectContent>
                        {BRAZILIAN_STATES.map((uf) => (
                          <SelectItem key={uf} value={uf}>{uf}</SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="zipCode">CEP</Label>
                    <Input
                      id="zipCode"
                      value={formData.zipCode}
                      onChange={(e) => setFormData({ ...formData, zipCode: formatCEP(e.target.value) })}
                      placeholder="00000-000"
                      data-testid="input-client-zip-code"
                    />
                  </div>
                </div>
              </TabsContent>

              <TabsContent value="financial" className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="bankName">Banco</Label>
                  <Input
                    id="bankName"
                    value={formData.bankName}
                    onChange={(e) => setFormData({ ...formData, bankName: e.target.value })}
                    placeholder="Nome do banco"
                    data-testid="input-client-bank-name"
                  />
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="bankAgency">Agência</Label>
                    <Input
                      id="bankAgency"
                      value={formData.bankAgency}
                      onChange={(e) => setFormData({ ...formData, bankAgency: e.target.value })}
                      placeholder="0000"
                      data-testid="input-client-bank-agency"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="bankAccount">Conta</Label>
                    <Input
                      id="bankAccount"
                      value={formData.bankAccount}
                      onChange={(e) => setFormData({ ...formData, bankAccount: e.target.value })}
                      placeholder="00000-0"
                      data-testid="input-client-bank-account"
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="pixKey">Chave PIX</Label>
                  <Input
                    id="pixKey"
                    value={formData.pixKey}
                    onChange={(e) => setFormData({ ...formData, pixKey: e.target.value })}
                    placeholder="CPF, CNPJ, e-mail, telefone ou chave aleatória"
                    data-testid="input-client-pix-key"
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="paymentTermDays">Prazo de Pagamento (dias)</Label>
                  <Input
                    id="paymentTermDays"
                    type="number"
                    min="0"
                    value={formData.paymentTermDays}
                    onChange={(e) => setFormData({ ...formData, paymentTermDays: parseInt(e.target.value) || 0 })}
                    placeholder="30"
                    data-testid="input-client-payment-term"
                  />
                </div>
              </TabsContent>

              {customFieldDefinitions.length > 0 && (
                <TabsContent value="custom" className="space-y-4">
                  <CustomFieldsSection
                    entityType="CLIENT"
                    entityId={client?.id}
                    values={customFieldValues}
                    onChange={setCustomFieldValues}
                  />
                </TabsContent>
              )}
            </Tabs>
          </ScrollArea>

          <DialogFooter className="mt-4 pt-4 border-t">
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={isLoading} data-testid="button-save-client">
              {isLoading && <Loader2 className="mr-2 size-4 animate-spin" />}
              {isEditing ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
