import { useState, useRef, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from "@/components/ui/sheet";
import { Sparkles, Send, Loader2, User, Package, Check, Plus, Building2, AlertCircle } from "lucide-react";
import { cn } from "@/lib/utils";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";
import { api } from "@/lib/api";
import { TactileCard } from "@/components/ui/tactile-card";
import { Badge } from "@/components/ui/badge";

interface Message {
  role: "user" | "assistant" | "system";
  content: string;
  data?: any;
  actions?: Array<{
    label: string;
    action: string;
    data?: any;
  }>;
}

interface ProposalCopilotProps {
  onCreateProposal?: (data: { clientId: string; items: Array<{ description: string; price: number; quantity: number; productId?: string }> }) => void;
}

export function ProposalCopilot({ onCreateProposal }: ProposalCopilotProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const [isOpen, setIsOpen] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [selectedClient, setSelectedClient] = useState<any>(null);
  const [selectedProduct, setSelectedProduct] = useState<any>(null);
  const [proposalData, setProposalData] = useState<any>(null);
  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollTop = scrollRef.current.scrollHeight;
    }
  }, [messages]);

  useEffect(() => {
    if (isOpen && messages.length === 0) {
      setMessages([{
        role: "assistant",
        content: "Olá! Sou o Copiloto de Propostas. Descreva a proposta que você deseja criar. Por exemplo:\n\n\"Faça uma proposta para o cliente Fabio Ribeiro sobre implantação de sistema de CRM por R$15.000\"",
      }]);
    }
  }, [isOpen, messages.length]);

  const analyzeText = async (text: string) => {
    if (!currentWorkspace) return;

    setIsLoading(true);
    try {
      const result = await api.analyzeProposalText(currentWorkspace.id, text);
      
      const newMessages: Message[] = [];
      
      if (result.entities.clientName) {
        if (result.clientFound) {
          const client = result.suggestions.clients[0];
          newMessages.push({
            role: "assistant",
            content: `Encontrei o cliente "${client.companyName || client.name}". Deseja usar este cliente?`,
            data: { client },
            actions: [
              { label: "Sim, usar este cliente", action: "confirm_client", data: client },
              { label: "Buscar outro", action: "search_client" }
            ]
          });
        } else if (result.clientSuggestions) {
          newMessages.push({
            role: "assistant",
            content: `Encontrei ${result.suggestions.clients.length} clientes que podem corresponder a "${result.entities.clientName}". Qual você deseja usar?`,
            data: { clients: result.suggestions.clients },
            actions: result.suggestions.clients.map((c: any) => ({
              label: c.companyName || c.name,
              action: "confirm_client",
              data: c
            }))
          });
        } else {
          newMessages.push({
            role: "assistant",
            content: `Não encontrei o cliente "${result.entities.clientName}". Deseja cadastrar um novo cliente?`,
            actions: [
              { label: "Cadastrar novo cliente", action: "create_client", data: { name: result.entities.clientName } },
              { label: "Buscar novamente", action: "search_client" }
            ]
          });
        }
      }

      if (result.entities.serviceName && selectedClient) {
        if (result.productFound) {
          const product = result.suggestions.products[0];
          newMessages.push({
            role: "assistant",
            content: `Encontrei o produto/serviço "${product.name}" (${formatCurrency(product.price)}). Deseja usar?`,
            data: { product },
            actions: [
              { label: "Sim, usar este produto", action: "confirm_product", data: product },
              { label: "Buscar outro", action: "search_product" }
            ]
          });
        } else if (result.productSuggestions) {
          newMessages.push({
            role: "assistant",
            content: `Encontrei ${result.suggestions.products.length} produtos/serviços similares. Qual você deseja usar?`,
            data: { products: result.suggestions.products },
            actions: result.suggestions.products.map((p: any) => ({
              label: `${p.name} - ${formatCurrency(p.price)}`,
              action: "confirm_product",
              data: p
            }))
          });
        } else {
          newMessages.push({
            role: "assistant",
            content: `Não encontrei um produto para "${result.entities.serviceName}". Deseja usar como item personalizado?`,
            data: { serviceName: result.entities.serviceName, value: result.entities.value },
            actions: [
              { label: "Usar item personalizado", action: "custom_item", data: { name: result.entities.serviceName, value: result.entities.value } },
              { label: "Buscar produto", action: "search_product" }
            ]
          });
        }
      }

      setProposalData({
        entities: result.entities,
        suggestions: result.suggestions
      });

      setMessages(prev => [...prev, ...newMessages]);
    } catch (error: any) {
      toast({
        title: "Erro",
        description: error.message || "Não foi possível analisar o texto",
        variant: "destructive",
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleAction = async (action: string, data?: any) => {
    switch (action) {
      case "confirm_client":
        const confirmedClient = data;
        setSelectedClient(confirmedClient);
        setMessages(prev => [...prev, {
          role: "system",
          content: `Cliente selecionado: ${confirmedClient.companyName || confirmedClient.name}`
        }]);
        
        if (proposalData?.entities?.serviceName) {
          const serviceTerms = proposalData.entities.serviceName.split(' ').filter((t: string) => t.length > 2);
          try {
            const products = await api.searchProductsForCopilot(currentWorkspace!.id, serviceTerms);
            if (products.length === 1) {
              setMessages(prev => [...prev, {
                role: "assistant",
                content: `Encontrei o serviço "${products[0].name}" (${formatCurrency(products[0].price)}). Deseja usar?`,
                data: { product: products[0] },
                actions: [
                  { label: "Sim, usar este serviço", action: "confirm_product", data: { ...products[0], _client: confirmedClient } },
                  { label: "Usar item personalizado", action: "custom_item", data: { name: proposalData.entities.serviceName, value: proposalData.entities.value, _client: confirmedClient } }
                ]
              }]);
            } else if (products.length > 1) {
              setMessages(prev => [...prev, {
                role: "assistant",
                content: `Encontrei ${products.length} serviços similares:`,
                data: { products },
                actions: [
                  ...products.slice(0, 5).map((p: any) => ({
                    label: `${p.name} - ${formatCurrency(p.price)}`,
                    action: "confirm_product",
                    data: { ...p, _client: confirmedClient }
                  })),
                  { label: "Usar item personalizado", action: "custom_item", data: { name: proposalData.entities.serviceName, value: proposalData.entities.value, _client: confirmedClient } }
                ]
              }]);
            } else {
              setMessages(prev => [...prev, {
                role: "assistant",
                content: `Não encontrei um produto cadastrado para "${proposalData.entities.serviceName}". Deseja criar um item personalizado?`,
                actions: [
                  { label: "Criar item personalizado", action: "custom_item", data: { name: proposalData.entities.serviceName, value: proposalData.entities.value, _client: confirmedClient } },
                  { label: "Cadastrar novo produto", action: "create_product", data: { name: proposalData.entities.serviceName } }
                ]
              }]);
            }
          } catch (error) {
            setMessages(prev => [...prev, {
              role: "assistant",
              content: "Descreva o serviço ou produto para a proposta:",
            }]);
          }
        } else {
          setMessages(prev => [...prev, {
            role: "assistant",
            content: "Agora me diga qual serviço ou produto deseja incluir na proposta:",
          }]);
        }
        break;

      case "confirm_product":
        const productClient = data._client || selectedClient;
        const productData = { ...data };
        delete productData._client;
        setSelectedProduct(productData);
        if (data._client) {
          setSelectedClient(data._client);
        }
        setMessages(prev => [...prev, {
          role: "system",
          content: `Produto selecionado: ${productData.name} - ${formatCurrency(productData.price)}`
        }, {
          role: "assistant",
          content: `Tudo pronto para criar a proposta:\n\n• Cliente: ${productClient?.companyName || productClient?.name}\n• Serviço: ${productData.name}\n• Valor: ${formatCurrency(productData.price)}\n\nDeseja criar a proposta agora?`,
          actions: [
            { label: "Criar Proposta", action: "create_proposal", data: { product: productData, _client: productClient } },
            { label: "Adicionar mais itens", action: "add_more_items" }
          ]
        }]);
        break;

      case "custom_item":
        const customItemClient = data._client || selectedClient;
        const customItem = {
          description: data.name,
          price: data.value || 0,
          quantity: 1
        };
        setSelectedProduct(customItem);
        if (data._client) {
          setSelectedClient(data._client);
        }
        setMessages(prev => [...prev, {
          role: "system",
          content: `Item personalizado: ${data.name}${data.value ? ` - ${formatCurrency(data.value)}` : ''}`
        }, {
          role: "assistant",
          content: data.value 
            ? `Perfeito! Proposta pronta:\n\n• Cliente: ${customItemClient?.companyName || customItemClient?.name}\n• Item: ${data.name}\n• Valor: ${formatCurrency(data.value)}\n\nDeseja criar agora?`
            : `Qual será o valor do serviço "${data.name}"?`,
          actions: data.value ? [
            { label: "Criar Proposta", action: "create_proposal", data: { custom: customItem, _client: customItemClient } },
            { label: "Alterar valor", action: "change_value" }
          ] : undefined
        }]);
        break;

      case "create_proposal":
        const proposalClient = data._client || selectedClient;
        if (proposalClient && onCreateProposal) {
          const items = data.product 
            ? [{ description: data.product.name, price: Number(data.product.price), quantity: 1, productId: data.product.id }]
            : data.custom 
              ? [data.custom]
              : [];
          
          onCreateProposal({
            clientId: proposalClient.id,
            items
          });
          
          toast({
            title: "Proposta criada!",
            description: "Você será redirecionado para editar a proposta.",
          });
          
          setIsOpen(false);
          resetState();
        }
        break;

      case "create_client":
        setMessages(prev => [...prev, {
          role: "assistant",
          content: `Para cadastrar o cliente "${data.name}", acesse a página de Clientes e crie um novo cadastro. Depois volte aqui para continuar.`,
        }]);
        break;

      case "search_client":
        setMessages(prev => [...prev, {
          role: "assistant",
          content: "Digite o nome do cliente que deseja buscar:",
        }]);
        break;

      case "search_product":
        setMessages(prev => [...prev, {
          role: "assistant",
          content: "Digite o nome do produto ou serviço que deseja buscar:",
        }]);
        break;
    }
  };

  const sendMessage = async () => {
    if (!input.trim() || isLoading || !currentWorkspace) return;

    const userMessage: Message = { role: "user", content: input.trim() };
    setMessages(prev => [...prev, userMessage]);
    const text = input.trim();
    setInput("");

    if (!selectedClient) {
      await analyzeText(text);
    } else if (!selectedProduct) {
      const terms = text.split(' ').filter(t => t.length > 2);
      try {
        setIsLoading(true);
        const products = await api.searchProductsForCopilot(currentWorkspace.id, terms);
        if (products.length > 0) {
          setMessages(prev => [...prev, {
            role: "assistant",
            content: `Encontrei ${products.length} resultado(s):`,
            actions: products.slice(0, 5).map((p: any) => ({
              label: `${p.name} - ${formatCurrency(p.price)}`,
              action: "confirm_product",
              data: p
            }))
          }]);
        } else {
          setMessages(prev => [...prev, {
            role: "assistant",
            content: `Não encontrei produtos para "${text}". Deseja usar como item personalizado?`,
            actions: [
              { label: "Usar item personalizado", action: "custom_item", data: { name: text, value: null } }
            ]
          }]);
        }
      } finally {
        setIsLoading(false);
      }
    }
  };

  const resetState = () => {
    setMessages([]);
    setSelectedClient(null);
    setSelectedProduct(null);
    setProposalData(null);
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  const formatCurrency = (value: number | string) => {
    const num = typeof value === 'string' ? parseFloat(value) : value;
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(num || 0);
  };

  return (
    <Sheet open={isOpen} onOpenChange={setIsOpen}>
      <SheetTrigger asChild>
        <Button
          className="gap-2 bg-gradient-to-r from-violet-600 to-purple-600 hover:from-violet-700 hover:to-purple-700 text-white shadow-lg"
          data-testid="button-proposal-copilot"
        >
          <Sparkles className="size-4" />
          Copiloto IA
        </Button>
      </SheetTrigger>
      <SheetContent className="w-full sm:max-w-lg flex flex-col p-0">
        <SheetHeader className="px-6 py-4 border-b bg-gradient-to-r from-violet-50 to-purple-50">
          <SheetTitle className="flex items-center gap-2">
            <Sparkles className="size-5 text-violet-600" />
            Copiloto de Propostas
          </SheetTitle>
          <p className="text-sm text-muted-foreground">
            Crie propostas rapidamente descrevendo o que você precisa
          </p>
        </SheetHeader>

        {(selectedClient || selectedProduct) && (
          <div className="px-4 py-2 border-b bg-muted/30 flex flex-wrap gap-2">
            {selectedClient && (
              <Badge variant="secondary" className="gap-1">
                <Building2 className="size-3" />
                {selectedClient.companyName || selectedClient.name}
              </Badge>
            )}
            {selectedProduct && (
              <Badge variant="secondary" className="gap-1">
                <Package className="size-3" />
                {selectedProduct.name || selectedProduct.description}
              </Badge>
            )}
          </div>
        )}

        <ScrollArea className="flex-1 px-4" ref={scrollRef}>
          <div className="py-4 space-y-4">
            {messages.map((message, index) => (
              <div key={index}>
                {message.role === "system" ? (
                  <div className="flex items-center gap-2 text-sm text-muted-foreground bg-muted/50 rounded-lg px-3 py-2">
                    <Check className="size-4 text-green-500" />
                    {message.content}
                  </div>
                ) : (
                  <div
                    className={cn(
                      "flex",
                      message.role === "user" ? "justify-end" : "justify-start"
                    )}
                  >
                    <div
                      className={cn(
                        "max-w-[85%] rounded-2xl px-4 py-3",
                        message.role === "user"
                          ? "bg-violet-600 text-white"
                          : "bg-muted"
                      )}
                    >
                      <p className="text-sm whitespace-pre-wrap">{message.content}</p>
                    </div>
                  </div>
                )}
                
                {message.actions && message.actions.length > 0 && (
                  <div className="mt-2 flex flex-wrap gap-2">
                    {message.actions.map((action, actionIndex) => (
                      <Button
                        key={actionIndex}
                        variant="outline"
                        size="sm"
                        onClick={() => handleAction(action.action, action.data)}
                        className="text-xs"
                        data-testid={`button-action-${action.action}`}
                      >
                        {action.action === "confirm_client" && <User className="size-3 mr-1" />}
                        {action.action === "confirm_product" && <Package className="size-3 mr-1" />}
                        {action.action === "create_proposal" && <Check className="size-3 mr-1" />}
                        {action.action.includes("create") && <Plus className="size-3 mr-1" />}
                        {action.label}
                      </Button>
                    ))}
                  </div>
                )}
              </div>
            ))}

            {isLoading && (
              <div className="flex justify-start">
                <div className="bg-muted rounded-2xl px-4 py-3">
                  <div className="flex items-center gap-2">
                    <Loader2 className="size-4 animate-spin text-violet-600" />
                    <span className="text-sm text-muted-foreground">Analisando...</span>
                  </div>
                </div>
              </div>
            )}
          </div>
        </ScrollArea>

        <div className="p-4 border-t">
          <div className="flex gap-2">
            <Input
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={handleKeyDown}
              placeholder={
                !selectedClient 
                  ? "Descreva a proposta que deseja criar..." 
                  : "Digite o serviço ou produto..."
              }
              disabled={isLoading}
              data-testid="input-copilot-message"
            />
            <Button
              size="icon"
              onClick={sendMessage}
              disabled={!input.trim() || isLoading}
              className="bg-violet-600 hover:bg-violet-700"
              data-testid="button-send-copilot"
            >
              <Send className="size-4" />
            </Button>
          </div>
          {messages.length > 1 && (
            <Button
              variant="ghost"
              size="sm"
              className="mt-2 text-xs text-muted-foreground"
              onClick={resetState}
            >
              Recomeçar conversa
            </Button>
          )}
        </div>
      </SheetContent>
    </Sheet>
  );
}
