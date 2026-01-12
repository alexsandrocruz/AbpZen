import { useState, useRef, useEffect } from "react";
import { useParams } from "wouter";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Separator } from "@/components/ui/separator";
import { Badge } from "@/components/ui/badge";
import { Spinner } from "@/components/ui/spinner";
import { useToast } from "@/hooks/use-toast";
import { CheckCircle2, FileText, Eraser, PenLine, Download, Printer } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { VisualProposalRenderer } from "@/components/proposal/visual-proposal-renderer";

function formatCurrency(value: number | string) {
  const num = typeof value === "string" ? parseFloat(value) : value;
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(num);
}

export default function PublicProposalPage() {
  const { token } = useParams<{ token: string }>();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const [isDrawing, setIsDrawing] = useState(false);
  const [signerName, setSignerName] = useState("");
  const [hasSignature, setHasSignature] = useState(false);

  const { data: proposalData, isLoading, error } = useQuery({
    queryKey: ["publicProposal", token],
    queryFn: () => api.getPublicProposal(token!),
    enabled: !!token,
  });

  const { data: visualData, isLoading: isVisualLoading, error: visualError } = useQuery({
    queryKey: ["publicProposalVisual", token],
    queryFn: () => api.getPublicProposalVisual(token!),
    enabled: !!token && !!proposalData?.proposal?.useVisualBuilder,
  });

  const signMutation = useMutation({
    mutationFn: (data: { signedByName: string; signatureData: string }) =>
      api.signProposal(token!, data),
    onSuccess: () => {
      toast({
        title: "Proposta assinada com sucesso!",
        description: "Obrigado por assinar a proposta.",
      });
      queryClient.invalidateQueries({ queryKey: ["publicProposal", token] });
    },
    onError: (error: Error) => {
      toast({
        variant: "destructive",
        title: "Erro ao assinar",
        description: error.message,
      });
    },
  });

  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;
    const ctx = canvas.getContext("2d");
    if (!ctx) return;
    ctx.fillStyle = "#fff";
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    ctx.strokeStyle = "#1e293b";
    ctx.lineWidth = 2;
    ctx.lineCap = "round";
    ctx.lineJoin = "round";
  }, [proposalData]);

  const getCoordinates = (e: React.MouseEvent | React.TouchEvent) => {
    const canvas = canvasRef.current;
    if (!canvas) return { x: 0, y: 0 };
    const rect = canvas.getBoundingClientRect();
    const scaleX = canvas.width / rect.width;
    const scaleY = canvas.height / rect.height;
    if ("touches" in e) {
      return {
        x: (e.touches[0].clientX - rect.left) * scaleX,
        y: (e.touches[0].clientY - rect.top) * scaleY,
      };
    }
    return {
      x: (e.clientX - rect.left) * scaleX,
      y: (e.clientY - rect.top) * scaleY,
    };
  };

  const startDrawing = (e: React.MouseEvent | React.TouchEvent) => {
    e.preventDefault();
    const canvas = canvasRef.current;
    const ctx = canvas?.getContext("2d");
    if (!ctx) return;
    const { x, y } = getCoordinates(e);
    ctx.beginPath();
    ctx.moveTo(x, y);
    setIsDrawing(true);
    setHasSignature(true);
  };

  const draw = (e: React.MouseEvent | React.TouchEvent) => {
    e.preventDefault();
    if (!isDrawing) return;
    const ctx = canvasRef.current?.getContext("2d");
    if (!ctx) return;
    const { x, y } = getCoordinates(e);
    ctx.lineTo(x, y);
    ctx.stroke();
  };

  const stopDrawing = () => {
    setIsDrawing(false);
  };

  const clearSignature = () => {
    const canvas = canvasRef.current;
    const ctx = canvas?.getContext("2d");
    if (!ctx || !canvas) return;
    ctx.fillStyle = "#fff";
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    setHasSignature(false);
  };

  const handleSign = () => {
    if (!signerName.trim()) {
      toast({
        variant: "destructive",
        title: "Nome obrigatório",
        description: "Por favor, informe seu nome completo.",
      });
      return;
    }
    if (!hasSignature) {
      toast({
        variant: "destructive",
        title: "Assinatura obrigatória",
        description: "Por favor, desenhe sua assinatura no campo indicado.",
      });
      return;
    }
    const canvas = canvasRef.current;
    if (!canvas) return;
    const signatureData = canvas.toDataURL("image/png");
    signMutation.mutate({ signedByName: signerName.trim(), signatureData });
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center" data-testid="loading-container">
        <Spinner className="h-8 w-8" />
      </div>
    );
  }

  if (error || !proposalData) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
        <Card className="w-full max-w-lg" data-testid="error-card">
          <CardContent className="pt-6 text-center">
            <FileText className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
            <h2 className="text-xl font-semibold mb-2">Proposta não encontrada</h2>
            <p className="text-muted-foreground">
              O link que você acessou é inválido ou expirou.
            </p>
          </CardContent>
        </Card>
      </div>
    );
  }

  const { proposal, client, items } = proposalData;
  const isSigned = !!proposal.signedAt;

  if (proposal.useVisualBuilder) {
    if (isVisualLoading) {
      return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center" data-testid="visual-loading-container">
          <Spinner className="h-8 w-8" />
        </div>
      );
    }

    if (visualError) {
      return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
          <Card className="w-full max-w-lg" data-testid="visual-error-card">
            <CardContent className="pt-6 text-center">
              <FileText className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
              <h2 className="text-xl font-semibold mb-2">Erro ao carregar proposta</h2>
              <p className="text-muted-foreground">
                Ocorreu um erro ao carregar os dados visuais da proposta.
              </p>
            </CardContent>
          </Card>
        </div>
      );
    }

    if (visualData?.blocks?.length > 0) {
      return (
        <VisualProposalRenderer
          proposal={visualData.proposal || proposal}
          client={visualData.client || client}
          items={visualData.items || items}
          blocks={visualData.blocks}
          workspace={visualData.workspace}
          isSigned={isSigned}
          onSign={(name, signatureData) => signMutation.mutate({ signedByName: name, signatureData })}
          isSignPending={signMutation.isPending}
        />
      );
    }
  }

  const calculateItemTotal = (item: any) => {
    const subtotal = parseFloat(item.price) * item.quantity;
    const discount = parseFloat(item.discount || "0");
    let total: number;
    if (item.discountType === "FIXED") {
      total = subtotal - Math.min(discount, subtotal);
    } else {
      const clampedDiscount = Math.min(Math.max(discount, 0), 100);
      total = subtotal * (1 - clampedDiscount / 100);
    }
    return Math.max(total, 0);
  };

  const subtotal = items?.reduce(
    (sum: number, item: any) => sum + calculateItemTotal(item),
    0
  ) || 0;

  const globalDiscount = parseFloat(proposal.globalDiscount || "0");
  const globalDiscountValue = proposal.globalDiscountType === "FIXED" 
    ? Math.min(Math.max(globalDiscount, 0), subtotal)
    : subtotal * (Math.min(Math.max(globalDiscount, 0), 100) / 100);
  const total = Math.max(subtotal - globalDiscountValue, 0);

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-3xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-slate-900" data-testid="logo-text">Dominus</h1>
        </div>

        <Card className="shadow-lg" data-testid="proposal-card">
          <CardHeader className="border-b">
            <div className="flex items-start justify-between">
              <div>
                <CardTitle className="text-2xl" data-testid="proposal-title">{proposal.title}</CardTitle>
                <p className="text-muted-foreground mt-1" data-testid="client-name">
                  Cliente: {client?.name || "Não informado"}
                </p>
                <p className="text-sm text-muted-foreground" data-testid="proposal-date">
                  Data: {format(new Date(proposal.createdAt), "dd 'de' MMMM 'de' yyyy", { locale: ptBR })}
                </p>
                {proposal.validUntil && (
                  <p className="text-sm text-muted-foreground" data-testid="valid-until">
                    Válida até: {format(new Date(proposal.validUntil), "dd/MM/yyyy", { locale: ptBR })}
                  </p>
                )}
              </div>
              <div className="flex flex-col items-end gap-2">
                <Badge
                  variant={isSigned ? "default" : "secondary"}
                  className={isSigned ? "bg-green-600" : ""}
                  data-testid="status-badge"
                >
                  {isSigned ? "Assinada" : "Aguardando assinatura"}
                </Badge>
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => window.print()}
                  className="print:hidden"
                  data-testid="button-print-pdf"
                >
                  <Printer className="h-4 w-4 mr-2" />
                  Imprimir / PDF
                </Button>
              </div>
            </div>
          </CardHeader>

          <CardContent className="pt-6 space-y-6">
            {proposal.content && (
              <div className="prose prose-sm max-w-none" data-testid="proposal-content">
                <p className="whitespace-pre-wrap">{proposal.content}</p>
              </div>
            )}

            <div>
              <h3 className="font-semibold text-lg mb-4">Itens da Proposta</h3>
              <div className="border rounded-lg overflow-hidden">
                <table className="w-full" data-testid="items-table">
                  <thead className="bg-muted/50">
                    <tr>
                      <th className="text-left p-3 font-medium">Descrição</th>
                      <th className="text-center p-3 font-medium w-20">Qtd.</th>
                      <th className="text-right p-3 font-medium w-28">Valor Unit.</th>
                      <th className="text-right p-3 font-medium w-24">Desc.</th>
                      <th className="text-right p-3 font-medium w-28">Total</th>
                    </tr>
                  </thead>
                  <tbody>
                    {items?.map((item: any, index: number) => {
                      const itemDiscount = parseFloat(item.discount || "0");
                      const hasDiscount = itemDiscount > 0;
                      return (
                        <tr key={item.id} className={index % 2 === 1 ? "bg-muted/20" : ""} data-testid={`item-row-${item.id}`}>
                          <td className="p-3">{item.description}</td>
                          <td className="p-3 text-center">{item.quantity}</td>
                          <td className="p-3 text-right">{formatCurrency(item.price)}</td>
                          <td className="p-3 text-right text-red-600">
                            {hasDiscount ? (
                              item.discountType === "FIXED" 
                                ? `- ${formatCurrency(itemDiscount)}`
                                : `${itemDiscount}%`
                            ) : "-"}
                          </td>
                          <td className="p-3 text-right font-medium">
                            {formatCurrency(calculateItemTotal(item))}
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                  <tfoot className="bg-muted/50">
                    {globalDiscountValue > 0 && (
                      <>
                        <tr>
                          <td colSpan={4} className="p-3 text-right">
                            Subtotal:
                          </td>
                          <td className="p-3 text-right">
                            {formatCurrency(subtotal)}
                          </td>
                        </tr>
                        <tr>
                          <td colSpan={4} className="p-3 text-right text-red-600">
                            Desconto ({proposal.globalDiscountType === "FIXED" ? formatCurrency(globalDiscount) : `${globalDiscount}%`}):
                          </td>
                          <td className="p-3 text-right text-red-600">
                            - {formatCurrency(globalDiscountValue)}
                          </td>
                        </tr>
                      </>
                    )}
                    <tr>
                      <td colSpan={4} className="p-3 text-right font-semibold">
                        Total da Proposta:
                      </td>
                      <td className="p-3 text-right font-bold text-lg" data-testid="proposal-total">
                        {formatCurrency(total)}
                      </td>
                    </tr>
                  </tfoot>
                </table>
              </div>
            </div>

            {proposal.contractText && (
              <div className="bg-muted/30 p-4 rounded-lg">
                <h4 className="font-semibold mb-2">Termos e Condições</h4>
                <p className="text-sm whitespace-pre-wrap">{proposal.contractText}</p>
              </div>
            )}

            <Separator />

            {isSigned ? (
              <div className="bg-green-50 border border-green-200 rounded-lg p-6 text-center" data-testid="signed-confirmation">
                <CheckCircle2 className="h-12 w-12 text-green-600 mx-auto mb-3" />
                <h3 className="text-lg font-semibold text-green-800 mb-2">
                  Proposta Assinada
                </h3>
                <p className="text-green-700">
                  Assinada por <strong>{proposal.signedByName}</strong>
                </p>
                <p className="text-green-600 text-sm mt-1">
                  Em {format(new Date(proposal.signedAt), "dd 'de' MMMM 'de' yyyy 'às' HH:mm", { locale: ptBR })}
                </p>
                {proposal.signatureData && (
                  <div className="mt-4">
                    <img
                      src={proposal.signatureData}
                      alt="Assinatura"
                      className="max-w-[200px] mx-auto border rounded"
                      data-testid="signature-image"
                    />
                  </div>
                )}
              </div>
            ) : (
              <div className="space-y-4" data-testid="signature-form">
                <h3 className="font-semibold text-lg flex items-center gap-2">
                  <PenLine className="h-5 w-5" />
                  Assinatura
                </h3>
                <p className="text-sm text-muted-foreground">
                  Para aceitar esta proposta, por favor preencha seu nome e assine abaixo.
                </p>

                <div className="space-y-2">
                  <Label htmlFor="signerName">Nome Completo *</Label>
                  <Input
                    id="signerName"
                    placeholder="Digite seu nome completo"
                    value={signerName}
                    onChange={(e) => setSignerName(e.target.value)}
                    data-testid="input-signer-name"
                  />
                </div>

                <div className="space-y-2">
                  <Label>Assinatura *</Label>
                  <div className="border rounded-lg p-2 bg-white">
                    <canvas
                      ref={canvasRef}
                      width={600}
                      height={200}
                      className="w-full h-[150px] cursor-crosshair touch-none border rounded"
                      onMouseDown={startDrawing}
                      onMouseMove={draw}
                      onMouseUp={stopDrawing}
                      onMouseLeave={stopDrawing}
                      onTouchStart={startDrawing}
                      onTouchMove={draw}
                      onTouchEnd={stopDrawing}
                      data-testid="signature-canvas"
                    />
                  </div>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={clearSignature}
                    className="mt-2"
                    data-testid="button-clear-signature"
                  >
                    <Eraser className="h-4 w-4 mr-2" />
                    Limpar Assinatura
                  </Button>
                </div>

                <Button
                  onClick={handleSign}
                  disabled={signMutation.isPending}
                  className="w-full"
                  size="lg"
                  data-testid="button-sign-proposal"
                >
                  {signMutation.isPending ? (
                    <>
                      <Spinner className="h-4 w-4 mr-2" />
                      Processando...
                    </>
                  ) : (
                    <>
                      <CheckCircle2 className="h-4 w-4 mr-2" />
                      Confirmar e Assinar Proposta
                    </>
                  )}
                </Button>
              </div>
            )}
          </CardContent>
        </Card>

        <p className="text-center text-sm text-muted-foreground mt-6">
          Sistema Dominus - Gestão de Negócios
        </p>
      </div>
    </div>
  );
}
