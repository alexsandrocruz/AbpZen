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
import { CheckCircle2, FileText, Eraser, PenLine, ScrollText } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

export default function PublicContractPage() {
  const { token } = useParams<{ token: string }>();
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const [isDrawing, setIsDrawing] = useState(false);
  const [signerName, setSignerName] = useState("");
  const [hasSignature, setHasSignature] = useState(false);

  const { data: contractData, isLoading, error } = useQuery({
    queryKey: ["publicContract", token],
    queryFn: () => api.getPublicContract(token!),
    enabled: !!token,
  });

  const signMutation = useMutation({
    mutationFn: (data: { signedByName: string; signatureData: string }) =>
      api.signContract(token!, data),
    onSuccess: () => {
      toast({
        title: "Contrato assinado com sucesso!",
        description: "Obrigado por assinar o contrato.",
      });
      queryClient.invalidateQueries({ queryKey: ["publicContract", token] });
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
  }, [contractData]);

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

  if (error || !contractData) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
        <Card className="w-full max-w-lg" data-testid="error-card">
          <CardContent className="pt-6 text-center">
            <ScrollText className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
            <h2 className="text-xl font-semibold mb-2">Contrato não encontrado</h2>
            <p className="text-muted-foreground">
              O link que você acessou é inválido ou expirou.
            </p>
          </CardContent>
        </Card>
      </div>
    );
  }

  const { contract, client } = contractData;
  const isSigned = !!contract.signedAt;

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-3xl mx-auto">
        <div className="text-center mb-8">
          <h1 className="text-3xl font-bold text-slate-900" data-testid="logo-text">Dominus</h1>
        </div>

        <Card className="shadow-lg" data-testid="contract-card">
          <CardHeader className="border-b">
            <div className="flex items-start justify-between">
              <div>
                <CardTitle className="text-2xl" data-testid="contract-title">{contract.title}</CardTitle>
                <p className="text-muted-foreground mt-1" data-testid="client-name">
                  Cliente: {client?.name || "Não informado"}
                </p>
                <p className="text-sm text-muted-foreground" data-testid="contract-date">
                  Data: {format(new Date(contract.createdAt), "dd 'de' MMMM 'de' yyyy", { locale: ptBR })}
                </p>
                {contract.expiresAt && (
                  <p className="text-sm text-muted-foreground" data-testid="expires-at">
                    Expira em: {format(new Date(contract.expiresAt), "dd/MM/yyyy", { locale: ptBR })}
                  </p>
                )}
              </div>
              <Badge
                variant={isSigned ? "default" : "secondary"}
                className={isSigned ? "bg-green-600" : ""}
                data-testid="status-badge"
              >
                {isSigned ? "Assinado" : "Aguardando assinatura"}
              </Badge>
            </div>
          </CardHeader>

          <CardContent className="pt-6 space-y-6">
            <div className="prose prose-sm max-w-none" data-testid="contract-content">
              <div
                className="whitespace-pre-wrap text-sm leading-relaxed"
                dangerouslySetInnerHTML={{ __html: contract.content.replace(/\n/g, "<br/>") }}
              />
            </div>

            <Separator />

            {isSigned ? (
              <div className="bg-green-50 border border-green-200 rounded-lg p-6 text-center" data-testid="signed-confirmation">
                <CheckCircle2 className="h-12 w-12 text-green-600 mx-auto mb-3" />
                <h3 className="text-lg font-semibold text-green-800 mb-2">
                  Contrato Assinado
                </h3>
                <p className="text-green-700">
                  Assinado por <strong>{contract.signedByName}</strong>
                </p>
                <p className="text-green-600 text-sm mt-1">
                  Em {format(new Date(contract.signedAt), "dd 'de' MMMM 'de' yyyy 'às' HH:mm", { locale: ptBR })}
                </p>
                {contract.signatureData && (
                  <div className="mt-4">
                    <img
                      src={contract.signatureData}
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
                  Para aceitar este contrato, por favor preencha seu nome e assine abaixo.
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
                  data-testid="button-sign-contract"
                >
                  {signMutation.isPending ? (
                    <>
                      <Spinner className="h-4 w-4 mr-2" />
                      Processando...
                    </>
                  ) : (
                    <>
                      <CheckCircle2 className="h-4 w-4 mr-2" />
                      Confirmar e Assinar Contrato
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
