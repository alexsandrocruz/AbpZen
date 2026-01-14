import { useState, useRef, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from "@/components/ui/sheet";
import { Bot, Send, Loader2, Sparkles, Copy, Check } from "lucide-react";
import { cn } from "@/lib/utils";
import { useAuth } from "@/lib/auth-store";
import { useToast } from "@/hooks/use-toast";

interface Message {
  role: "user" | "assistant";
  content: string;
}

interface ProposalChatbotProps {
  clientInfo?: {
    name?: string;
    company?: string;
    email?: string;
    phone?: string;
  };
  onInsertContent?: (content: string) => void;
}

export function ProposalChatbot({ clientInfo, onInsertContent }: ProposalChatbotProps) {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const [isOpen, setIsOpen] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [copiedIndex, setCopiedIndex] = useState<number | null>(null);
  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollTop = scrollRef.current.scrollHeight;
    }
  }, [messages]);

  const sendMessage = async () => {
    if (!input.trim() || isLoading || !currentWorkspace) return;

    const userMessage: Message = { role: "user", content: input.trim() };
    setMessages((prev) => [...prev, userMessage]);
    setInput("");
    setIsLoading(true);

    try {
      const response = await fetch(`/api/workspaces/${currentWorkspace.id}/ai/proposal-assistant`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({
          messages: [...messages, userMessage],
          clientInfo,
        }),
      });

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || "Erro ao enviar mensagem");
      }

      const data = await response.json();
      setMessages((prev) => [...prev, { role: "assistant", content: data.message }]);
    } catch (error: any) {
      toast({
        title: "Erro",
        description: error.message || "Não foi possível processar sua mensagem",
        variant: "destructive",
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  const copyToClipboard = async (text: string, index: number) => {
    await navigator.clipboard.writeText(text);
    setCopiedIndex(index);
    setTimeout(() => setCopiedIndex(null), 2000);
  };

  const insertContent = (content: string) => {
    if (onInsertContent) {
      onInsertContent(content);
      toast({ title: "Conteúdo inserido", description: "O texto foi adicionado à proposta" });
    }
  };

  const suggestedPrompts = [
    "Sugira um título atrativo para minha proposta",
    "Escreva uma introdução profissional",
    "Liste os principais benefícios do serviço",
    "Crie termos e condições básicos",
  ];

  return (
    <Sheet open={isOpen} onOpenChange={setIsOpen}>
      <SheetTrigger asChild>
        <Button
          variant="outline"
          size="sm"
          className="gap-2 bg-gradient-to-r from-violet-500/10 to-purple-500/10 border-violet-200 hover:border-violet-300 hover:bg-violet-50"
          data-testid="button-ai-assistant"
        >
          <Sparkles className="size-4 text-violet-600" />
          Assistente IA
        </Button>
      </SheetTrigger>
      <SheetContent className="w-full sm:max-w-lg flex flex-col p-0">
        <SheetHeader className="px-6 py-4 border-b bg-gradient-to-r from-violet-50 to-purple-50">
          <SheetTitle className="flex items-center gap-2">
            <Bot className="size-5 text-violet-600" />
            Assistente de Propostas
          </SheetTitle>
          <p className="text-sm text-muted-foreground">
            Tire dúvidas e receba ajuda para criar propostas profissionais.
          </p>
        </SheetHeader>

        <ScrollArea className="flex-1 p-4" ref={scrollRef}>
          <div className="space-y-4">
            {messages.length === 0 && (
              <div className="space-y-4">
                <div className="bg-violet-50 rounded-lg p-4 text-sm">
                  <p className="text-violet-800 font-medium mb-2">Olá! Sou seu assistente de propostas.</p>
                  <p className="text-violet-600">
                    Posso ajudar você a criar títulos, escrever descrições, sugerir preços e muito mais.
                    {clientInfo?.name && ` Vejo que você está criando uma proposta para ${clientInfo.name}.`}
                  </p>
                </div>
                <div className="space-y-2">
                  <p className="text-xs text-muted-foreground font-medium">Sugestões:</p>
                  {suggestedPrompts.map((prompt, i) => (
                    <Button
                      key={i}
                      variant="outline"
                      size="sm"
                      className="w-full justify-start text-left h-auto py-2 px-3"
                      onClick={() => {
                        setInput(prompt);
                      }}
                      data-testid={`button-prompt-${i}`}
                    >
                      {prompt}
                    </Button>
                  ))}
                </div>
              </div>
            )}

            {messages.map((message, index) => (
              <div
                key={index}
                className={cn(
                  "flex",
                  message.role === "user" ? "justify-end" : "justify-start"
                )}
              >
                <div
                  className={cn(
                    "max-w-[85%] rounded-lg px-4 py-2 text-sm",
                    message.role === "user"
                      ? "bg-violet-600 text-white"
                      : "bg-muted"
                  )}
                >
                  <div className="whitespace-pre-wrap">{message.content}</div>
                  {message.role === "assistant" && (
                    <div className="flex gap-1 mt-2 pt-2 border-t border-border/50">
                      <Button
                        variant="ghost"
                        size="sm"
                        className="h-7 text-xs"
                        onClick={() => copyToClipboard(message.content, index)}
                        data-testid={`button-copy-${index}`}
                      >
                        {copiedIndex === index ? (
                          <Check className="size-3 mr-1" />
                        ) : (
                          <Copy className="size-3 mr-1" />
                        )}
                        {copiedIndex === index ? "Copiado" : "Copiar"}
                      </Button>
                      {onInsertContent && (
                        <Button
                          variant="ghost"
                          size="sm"
                          className="h-7 text-xs"
                          onClick={() => insertContent(message.content)}
                          data-testid={`button-insert-${index}`}
                        >
                          <Sparkles className="size-3 mr-1" />
                          Inserir
                        </Button>
                      )}
                    </div>
                  )}
                </div>
              </div>
            ))}

            {isLoading && (
              <div className="flex justify-start">
                <div className="bg-muted rounded-lg px-4 py-2">
                  <Loader2 className="size-4 animate-spin text-violet-600" />
                </div>
              </div>
            )}
          </div>
        </ScrollArea>

        <div className="p-4 border-t bg-background">
          <div className="flex gap-2">
            <Input
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={handleKeyDown}
              placeholder="Digite sua pergunta..."
              disabled={isLoading}
              className="flex-1"
              data-testid="input-chat-message"
            />
            <Button
              onClick={sendMessage}
              disabled={!input.trim() || isLoading}
              size="icon"
              className="bg-violet-600 hover:bg-violet-700"
              data-testid="button-send-message"
            >
              <Send className="size-4" />
            </Button>
          </div>
        </div>
      </SheetContent>
    </Sheet>
  );
}
