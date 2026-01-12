import { useRef, useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Badge } from "@/components/ui/badge";
import { useToast } from "@/hooks/use-toast";
import { CheckCircle2, Eraser, PenLine, Printer } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

interface Block {
  id: string;
  blockType: string;
  position: number;
  content: string;
  style: string;
  isVisible: boolean;
}

interface VisualProposalRendererProps {
  proposal: any;
  client: any;
  items: any[];
  blocks: Block[];
  workspace?: any;
  isSigned: boolean;
  onSign: (name: string, signatureData: string) => void;
  isSignPending?: boolean;
}

function formatCurrency(value: number | string) {
  const num = typeof value === "string" ? parseFloat(value) : value;
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(num);
}

function parseContent(content: string): Record<string, any> {
  try {
    return JSON.parse(content);
  } catch {
    return {};
  }
}

function parseStyle(style: string): Record<string, any> {
  try {
    return JSON.parse(style);
  } catch {
    return {};
  }
}

function replaceVariables(text: string, variables: Record<string, any>): string {
  if (!text) return '';
  return text.replace(/\{\{([^}]+)\}\}/g, (match, key) => {
    const keys = key.trim().split('.');
    let value: any = variables;
    for (const k of keys) {
      value = value?.[k];
      if (value === undefined) return match;
    }
    return String(value ?? match);
  });
}

function BlockRenderer({ 
  block, 
  variables, 
  items,
  subtotal,
  total,
  globalDiscountValue,
  proposal,
}: { 
  block: Block; 
  variables: Record<string, any>;
  items: any[];
  subtotal: number;
  total: number;
  globalDiscountValue: number;
  proposal: any;
}) {
  const content = parseContent(block.content);
  const style = parseStyle(block.style);
  const templateStyle = proposal.templateStyle ? parseStyle(proposal.templateStyle) : {};
  
  const primaryColor = templateStyle.primaryColor || '#22c55e';
  const secondaryColor = templateStyle.secondaryColor || '#eab308';

  if (!block.isVisible) return null;

  switch (block.blockType) {
    case 'HERO':
      return (
        <div 
          className="py-16 px-8 text-center rounded-lg"
          style={{ 
            background: `linear-gradient(135deg, ${primaryColor} 0%, ${secondaryColor} 100%)`,
            color: 'white',
          }}
        >
          <h1 className="text-4xl font-bold mb-4">
            {replaceVariables(content.title || '{{proposal.title}}', variables)}
          </h1>
          <p className="text-xl opacity-90">
            {replaceVariables(content.subtitle || 'Proposta para {{client.name}}', variables)}
          </p>
          {content.showDate && (
            <p className="mt-6 opacity-80">
              {format(new Date(), "dd 'de' MMMM 'de' yyyy", { locale: ptBR })}
            </p>
          )}
        </div>
      );

    case 'ABOUT':
      return (
        <div className="py-8">
          <h2 className="text-2xl font-bold mb-4" style={{ color: primaryColor }}>
            {content.heading || 'Sobre Nós'}
          </h2>
          <p className="text-gray-700 mb-4">{content.description}</p>
          {content.highlights?.length > 0 && (
            <div className="grid grid-cols-3 gap-4 mt-6">
              {content.highlights.map((h: string, i: number) => (
                <div key={i} className="text-center p-4 bg-gray-50 rounded-lg">
                  <CheckCircle2 className="h-6 w-6 mx-auto mb-2" style={{ color: primaryColor }} />
                  <span className="text-sm font-medium">{h}</span>
                </div>
              ))}
            </div>
          )}
        </div>
      );

    case 'NEED':
      return (
        <div className="py-8">
          <h2 className="text-2xl font-bold mb-4" style={{ color: primaryColor }}>
            {content.heading || 'A Necessidade'}
          </h2>
          <p className="text-gray-700">
            {replaceVariables(content.description || '', variables)}
          </p>
        </div>
      );

    case 'SERVICES_TABLE':
      return (
        <div className="py-8">
          <h2 className="text-2xl font-bold mb-4" style={{ color: primaryColor }}>
            Serviços
          </h2>
          <div className="border rounded-lg overflow-hidden">
            <table className="w-full">
              <thead style={{ backgroundColor: `${primaryColor}15` }}>
                <tr>
                  <th className="text-left p-3 font-medium">Descrição</th>
                  {content.showQuantity !== false && (
                    <th className="text-center p-3 font-medium w-20">Qtd.</th>
                  )}
                  <th className="text-right p-3 font-medium w-28">Valor</th>
                  {content.showDiscount !== false && (
                    <th className="text-right p-3 font-medium w-24">Desc.</th>
                  )}
                  <th className="text-right p-3 font-medium w-28">Total</th>
                </tr>
              </thead>
              <tbody>
                {items.map((item: any, index: number) => {
                  const itemSubtotal = parseFloat(item.price) * item.quantity;
                  const discount = parseFloat(item.discount || "0");
                  const discountValue = item.discountType === "FIXED" 
                    ? Math.min(discount, itemSubtotal) 
                    : itemSubtotal * (Math.min(discount, 100) / 100);
                  const itemTotal = Math.max(itemSubtotal - discountValue, 0);
                  
                  return (
                    <tr key={item.id} className={index % 2 === 1 ? "bg-gray-50" : ""}>
                      <td className="p-3">{item.description}</td>
                      {content.showQuantity !== false && (
                        <td className="p-3 text-center">{item.quantity}</td>
                      )}
                      <td className="p-3 text-right">{formatCurrency(item.price)}</td>
                      {content.showDiscount !== false && (
                        <td className="p-3 text-right text-red-600">
                          {discount > 0 && (item.discountType === "FIXED" ? formatCurrency(discount) : `${discount}%`)}
                        </td>
                      )}
                      <td className="p-3 text-right font-medium">{formatCurrency(itemTotal)}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      );

    case 'INVESTMENT':
      return (
        <div 
          className="py-8 px-6 rounded-lg text-center"
          style={{ backgroundColor: `${primaryColor}10` }}
        >
          <h2 className="text-2xl font-bold mb-4" style={{ color: primaryColor }}>
            {content.heading || 'Investimento Total'}
          </h2>
          {content.showDiscount && globalDiscountValue > 0 && (
            <div className="space-y-2 mb-4">
              <p className="text-gray-600">Subtotal: {formatCurrency(subtotal)}</p>
              <p className="text-red-600">Desconto: -{formatCurrency(globalDiscountValue)}</p>
            </div>
          )}
          <p className="text-4xl font-bold" style={{ color: primaryColor }}>
            {formatCurrency(total)}
          </p>
          {content.showValidUntil && proposal.validUntil && (
            <p className="text-sm text-gray-500 mt-4">
              Válido até {format(new Date(proposal.validUntil), "dd/MM/yyyy", { locale: ptBR })}
            </p>
          )}
        </div>
      );

    case 'PORTFOLIO':
      return (
        <div className="py-8">
          <h2 className="text-2xl font-bold mb-4" style={{ color: primaryColor }}>
            {content.heading || 'Portfólio'}
          </h2>
          {content.images?.length > 0 && (
            <div className={`grid grid-cols-${content.columns || 3} gap-4`}>
              {content.images.map((url: string, i: number) => (
                <img key={i} src={url} alt="" className="rounded-lg w-full h-40 object-cover" />
              ))}
            </div>
          )}
        </div>
      );

    case 'TESTIMONIAL':
      return (
        <div className="py-8 px-6 bg-gray-50 rounded-lg">
          <blockquote className="text-xl italic text-gray-700 border-l-4 pl-4" style={{ borderColor: primaryColor }}>
            "{content.quote}"
          </blockquote>
          <div className="mt-4">
            <p className="font-semibold">{content.author}</p>
            {content.role && <p className="text-sm text-gray-500">{content.role}</p>}
          </div>
        </div>
      );

    case 'TERMS':
      return (
        <div className="py-8">
          <h2 className="text-2xl font-bold mb-4" style={{ color: primaryColor }}>
            {content.heading || 'Termos e Condições'}
          </h2>
          <div className="bg-gray-50 p-6 rounded-lg text-sm text-gray-700 whitespace-pre-wrap">
            {content.text}
          </div>
        </div>
      );

    case 'TEXT':
      return (
        <div className="py-4" style={{ textAlign: style.textAlign as any || 'left' }}>
          <p className="text-gray-700 whitespace-pre-wrap">
            {replaceVariables(content.text || '', variables)}
          </p>
        </div>
      );

    case 'IMAGE':
      return (
        <div className="py-4">
          {content.url && (
            <figure>
              <img src={content.url} alt={content.alt || ''} className="rounded-lg w-full" />
              {content.caption && (
                <figcaption className="text-center text-sm text-gray-500 mt-2">
                  {content.caption}
                </figcaption>
              )}
            </figure>
          )}
        </div>
      );

    case 'TWO_COLUMNS':
      return (
        <div className="py-4 grid grid-cols-2 gap-6">
          <div className="text-gray-700">{replaceVariables(content.leftContent || '', variables)}</div>
          <div className="text-gray-700">{replaceVariables(content.rightContent || '', variables)}</div>
        </div>
      );

    case 'DIVIDER':
      return (
        <hr 
          className={`my-${style.margin === 'lg' ? '12' : style.margin === 'sm' ? '4' : '8'}`}
          style={{ borderStyle: style.dividerStyle || 'solid' }}
        />
      );

    case 'SIGNATURE':
      return null;

    default:
      return null;
  }
}

export function VisualProposalRenderer({
  proposal,
  client,
  items,
  blocks,
  workspace,
  isSigned,
  onSign,
  isSignPending,
}: VisualProposalRendererProps) {
  const { toast } = useToast();
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const [isDrawing, setIsDrawing] = useState(false);
  const [signerName, setSignerName] = useState("");
  const [hasSignature, setHasSignature] = useState(false);

  const templateStyle = proposal.templateStyle ? parseStyle(proposal.templateStyle) : {};
  const primaryColor = templateStyle.primaryColor || '#22c55e';

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

  const variables = {
    proposal: {
      title: proposal.title,
      total: formatCurrency(total),
      subtotal: formatCurrency(subtotal),
      itemCount: items?.length || 0,
      validUntil: proposal.validUntil ? format(new Date(proposal.validUntil), "dd/MM/yyyy", { locale: ptBR }) : 'N/A',
    },
    client: {
      name: client?.companyName || client?.name || 'Cliente',
      email: client?.email || '',
      phone: client?.phone || '',
    },
    workspace: {
      name: workspace?.name || 'Empresa',
    },
  };

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
  }, []);

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
    const ctx = canvasRef.current?.getContext("2d");
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
    onSign(signerName.trim(), signatureData);
  };

  const sortedBlocks = [...blocks].sort((a, b) => a.position - b.position);
  const signatureBlock = sortedBlocks.find(b => b.blockType === 'SIGNATURE');
  const signatureContent = signatureBlock ? parseContent(signatureBlock.content) : {};

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-4xl mx-auto">
        <div className="flex justify-between items-center mb-8 print:hidden">
          <h1 className="text-2xl font-bold" style={{ color: primaryColor }}>
            {workspace?.name || 'Dominus'}
          </h1>
          <div className="flex items-center gap-4">
            <Badge
              variant={isSigned ? "default" : "secondary"}
              className={isSigned ? "bg-green-600" : ""}
            >
              {isSigned ? "Assinada" : "Aguardando assinatura"}
            </Badge>
            <Button
              variant="outline"
              size="sm"
              onClick={() => window.print()}
            >
              <Printer className="h-4 w-4 mr-2" />
              Imprimir / PDF
            </Button>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow-lg overflow-hidden">
          <div className="space-y-0">
            {sortedBlocks.map((block) => (
              <div key={block.id} className="px-8">
                <BlockRenderer
                  block={block}
                  variables={variables}
                  items={items}
                  subtotal={subtotal}
                  total={total}
                  globalDiscountValue={globalDiscountValue}
                  proposal={proposal}
                />
              </div>
            ))}
          </div>

          {!isSigned && signatureBlock && (
            <div className="px-8 py-8 border-t print:hidden">
              <h2 className="text-2xl font-bold mb-6" style={{ color: primaryColor }}>
                Assinatura
              </h2>
              
              <div className="grid md:grid-cols-2 gap-8">
                <div>
                  <div className="text-center p-6 bg-gray-50 rounded-lg">
                    <p className="font-medium mb-2">
                      {replaceVariables(signatureContent.companyName || '{{workspace.name}}', variables)}
                    </p>
                    <div className="border-t-2 border-dashed border-gray-300 w-48 mx-auto mt-12 pt-2">
                      <span className="text-sm text-gray-500">Empresa</span>
                    </div>
                  </div>
                </div>

                <div>
                  <div className="space-y-4">
                    <div>
                      <Label htmlFor="signerName">Seu Nome Completo</Label>
                      <Input
                        id="signerName"
                        value={signerName}
                        onChange={(e) => setSignerName(e.target.value)}
                        placeholder="Digite seu nome completo"
                        disabled={isSignPending}
                      />
                    </div>

                    <div>
                      <Label>Assinatura Digital</Label>
                      <div className="border-2 border-dashed border-gray-300 rounded-lg p-2 relative">
                        <canvas
                          ref={canvasRef}
                          width={400}
                          height={150}
                          className="w-full touch-none cursor-crosshair bg-white rounded"
                          onMouseDown={startDrawing}
                          onMouseMove={draw}
                          onMouseUp={stopDrawing}
                          onMouseLeave={stopDrawing}
                          onTouchStart={startDrawing}
                          onTouchMove={draw}
                          onTouchEnd={stopDrawing}
                        />
                        <Button
                          type="button"
                          variant="ghost"
                          size="sm"
                          className="absolute top-2 right-2"
                          onClick={clearSignature}
                        >
                          <Eraser className="h-4 w-4 mr-1" />
                          Limpar
                        </Button>
                      </div>
                      <p className="text-xs text-gray-500 mt-1">
                        Desenhe sua assinatura no campo acima
                      </p>
                    </div>

                    <Button
                      onClick={handleSign}
                      disabled={isSignPending}
                      className="w-full"
                      style={{ backgroundColor: primaryColor }}
                    >
                      <PenLine className="h-4 w-4 mr-2" />
                      {isSignPending ? "Assinando..." : "Assinar Proposta"}
                    </Button>
                  </div>
                </div>
              </div>
            </div>
          )}

          {isSigned && (
            <div className="px-8 py-8 border-t bg-green-50">
              <div className="flex items-center justify-center gap-4">
                <CheckCircle2 className="h-8 w-8 text-green-600" />
                <div>
                  <p className="text-lg font-semibold text-green-800">Proposta Assinada</p>
                  <p className="text-sm text-green-600">
                    Assinado por {proposal.signedByName} em{" "}
                    {format(new Date(proposal.signedAt), "dd/MM/yyyy 'às' HH:mm", { locale: ptBR })}
                  </p>
                </div>
              </div>
              {proposal.signatureData && (
                <div className="mt-4 flex justify-center">
                  <img
                    src={proposal.signatureData}
                    alt="Assinatura"
                    className="h-20 border rounded bg-white p-2"
                  />
                </div>
              )}
            </div>
          )}
        </div>

        <p className="text-center text-sm text-gray-400 mt-8">
          Powered by <span style={{ color: primaryColor }}>Dominus</span>
        </p>
      </div>
    </div>
  );
}
