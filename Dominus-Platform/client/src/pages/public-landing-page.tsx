import { useState, useEffect } from "react";
import { useParams, useSearch } from "wouter";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Spinner } from "@/components/ui/spinner";
import { CheckCircle2, FileX2, Users, Share2, Copy } from "lucide-react";

interface FormField {
  id: string;
  type: string;
  label: string;
  placeholder?: string;
  required: boolean;
  options?: string;
  position: number;
}

interface LandingPageData {
  id: string;
  slug: string;
  title: string;
  subtitle?: string;
  description?: string;
  logoUrl?: string;
  primaryColor: string;
  backgroundColor: string;
  showSocialProof: boolean;
  socialProofText?: string;
  footerText?: string;
  form?: {
    id: string;
    submitButtonText: string;
    successMessage: string;
    fields: FormField[];
  };
}

interface SubmitResponse {
  message: string;
  referralCode: string;
  queuePosition?: number;
}

export default function PublicLandingPage() {
  const { slug } = useParams<{ slug: string }>();
  const search = useSearch();
  const searchParams = new URLSearchParams(search);
  const referredBy = searchParams.get("ref") || "";

  const [landingPage, setLandingPage] = useState<LandingPageData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<Record<string, any>>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitResult, setSubmitResult] = useState<SubmitResponse | null>(null);
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (!slug) return;

    const fetchLandingPage = async () => {
      try {
        const response = await fetch(`/api/public/landing/${slug}`);
        if (!response.ok) {
          if (response.status === 404) {
            setError("not_found");
          } else {
            setError("error");
          }
          return;
        }
        const data = await response.json();
        setLandingPage(data);
      } catch (err) {
        setError("error");
      } finally {
        setIsLoading(false);
      }
    };

    fetchLandingPage();
  }, [slug]);

  const handleInputChange = (fieldId: string, value: any) => {
    setFormData((prev) => ({ ...prev, [fieldId]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!landingPage?.form) return;

    setIsSubmitting(true);

    try {
      const submitData: Record<string, any> = {};

      landingPage.form.fields.forEach((field) => {
        const value = formData[field.id];
        if (field.type === "EMAIL" && field.label.toLowerCase().includes("email")) {
          submitData.email = value;
        } else if (field.type === "PHONE" || field.label.toLowerCase().includes("telefone") || field.label.toLowerCase().includes("phone")) {
          submitData.phone = value;
        } else if (field.type === "TEXT" && (field.label.toLowerCase().includes("nome") || field.label.toLowerCase().includes("name"))) {
          submitData.name = value;
        } else {
          submitData[field.label] = value;
        }
      });

      if (referredBy) {
        submitData.referredBy = referredBy;
      }

      const response = await fetch(`/api/public/landing/${slug}/submit`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(submitData),
      });

      if (!response.ok) {
        const errData = await response.json();
        throw new Error(errData.message || "Erro ao enviar formulário");
      }

      const result = await response.json();
      setSubmitResult(result);
    } catch (err: any) {
      alert(err.message || "Erro ao enviar formulário");
    } finally {
      setIsSubmitting(false);
    }
  };

  const copyReferralLink = () => {
    if (!submitResult?.referralCode) return;
    const link = `${window.location.origin}/l/${slug}?ref=${submitResult.referralCode}`;
    navigator.clipboard.writeText(link);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  const renderField = (field: FormField) => {
    const value = formData[field.id] ?? "";
    const commonProps = {
      id: field.id,
      required: field.required,
      "data-testid": `input-${field.id}`,
    };

    switch (field.type) {
      case "TEXT":
        return (
          <Input
            {...commonProps}
            type="text"
            placeholder={field.placeholder}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );

      case "EMAIL":
        return (
          <Input
            {...commonProps}
            type="email"
            placeholder={field.placeholder || "seu@email.com"}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );

      case "PHONE":
        return (
          <Input
            {...commonProps}
            type="tel"
            placeholder={field.placeholder || "(11) 99999-9999"}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );

      case "NUMBER":
        return (
          <Input
            {...commonProps}
            type="number"
            placeholder={field.placeholder}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );

      case "URL":
        return (
          <Input
            {...commonProps}
            type="url"
            placeholder={field.placeholder || "https://"}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );

      case "DATE":
        return (
          <Input
            {...commonProps}
            type="date"
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );

      case "TEXTAREA":
        return (
          <Textarea
            {...commonProps}
            placeholder={field.placeholder}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
            rows={4}
          />
        );

      case "CHECKBOX":
        return (
          <div className="flex items-center space-x-2">
            <Checkbox
              id={field.id}
              checked={!!value}
              onCheckedChange={(checked) => handleInputChange(field.id, checked)}
              data-testid={`input-${field.id}`}
            />
            <label
              htmlFor={field.id}
              className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
            >
              {field.label}
            </label>
          </div>
        );

      case "SELECT":
        let options: string[] = [];
        if (field.options) {
          try {
            const parsed = JSON.parse(field.options);
            options = Array.isArray(parsed) ? parsed : [];
          } catch {
            options = field.options.split(',').map((s: string) => s.trim()).filter(Boolean);
          }
        }
        return (
          <Select
            value={value}
            onValueChange={(val) => handleInputChange(field.id, val)}
          >
            <SelectTrigger data-testid={`input-${field.id}`}>
              <SelectValue placeholder={field.placeholder || "Selecione..."} />
            </SelectTrigger>
            <SelectContent>
              {options.map((opt: string, i: number) => (
                <SelectItem key={i} value={opt}>
                  {opt}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        );

      default:
        return (
          <Input
            {...commonProps}
            type="text"
            placeholder={field.placeholder}
            value={value}
            onChange={(e) => handleInputChange(field.id, e.target.value)}
          />
        );
    }
  };

  if (isLoading) {
    return (
      <div
        className="min-h-screen flex items-center justify-center"
        data-testid="loading-container"
      >
        <Spinner className="h-8 w-8" />
      </div>
    );
  }

  if (error || !landingPage) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center p-4">
        <Card className="w-full max-w-lg" data-testid="error-card">
          <CardContent className="pt-6 text-center">
            <FileX2 className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
            <h2 className="text-xl font-semibold mb-2">Página não encontrada</h2>
            <p className="text-muted-foreground">
              O link que você acessou é inválido ou a página não está mais disponível.
            </p>
          </CardContent>
        </Card>
      </div>
    );
  }

  const primaryColor = landingPage.primaryColor || "#6366f1";
  const backgroundColor = landingPage.backgroundColor || "#ffffff";

  if (submitResult) {
    const referralLink = `${window.location.origin}/l/${slug}?ref=${submitResult.referralCode}`;

    return (
      <div
        className="min-h-screen flex items-center justify-center p-4"
        style={{ backgroundColor }}
      >
        <Card className="w-full max-w-lg shadow-lg" data-testid="success-card">
          <CardContent className="pt-8 pb-8 text-center space-y-6">
            <div
              className="w-16 h-16 rounded-full mx-auto flex items-center justify-center"
              style={{ backgroundColor: primaryColor }}
            >
              <CheckCircle2 className="h-8 w-8 text-white" />
            </div>

            <div>
              <h2 className="text-2xl font-bold mb-2" data-testid="success-message">
                {submitResult.message}
              </h2>
              {submitResult.queuePosition && (
                <p className="text-muted-foreground" data-testid="queue-position">
                  Sua posição na fila: <strong>#{submitResult.queuePosition}</strong>
                </p>
              )}
            </div>

            <div className="bg-muted/50 rounded-lg p-4 space-y-3">
              <div className="flex items-center justify-center gap-2 text-sm text-muted-foreground">
                <Share2 className="h-4 w-4" />
                <span>Compartilhe e ganhe benefícios!</span>
              </div>

              <div className="flex items-center gap-2">
                <Input
                  readOnly
                  value={referralLink}
                  className="text-sm"
                  data-testid="referral-link"
                />
                <Button
                  variant="outline"
                  size="icon"
                  onClick={copyReferralLink}
                  data-testid="button-copy-link"
                >
                  <Copy className="h-4 w-4" />
                </Button>
              </div>

              {copied && (
                <p className="text-sm text-green-600" data-testid="copied-message">
                  Link copiado!
                </p>
              )}

              <p className="text-xs text-muted-foreground">
                Seu código de indicação: <strong>{submitResult.referralCode}</strong>
              </p>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  const sortedFields = landingPage.form?.fields
    ? [...landingPage.form.fields].sort((a, b) => a.position - b.position)
    : [];

  return (
    <div className="min-h-screen" style={{ backgroundColor }}>
      <div className="max-w-2xl mx-auto px-4 py-8 md:py-16">
        {landingPage.logoUrl && (
          <div className="text-center mb-8">
            <img
              src={landingPage.logoUrl}
              alt="Logo"
              className="h-16 mx-auto"
              data-testid="landing-logo"
            />
          </div>
        )}

        <div className="text-center mb-8">
          <h1
            className="text-3xl md:text-4xl font-bold mb-4"
            style={{ color: primaryColor }}
            data-testid="landing-title"
          >
            {landingPage.title}
          </h1>

          {landingPage.subtitle && (
            <p
              className="text-xl text-muted-foreground mb-4"
              data-testid="landing-subtitle"
            >
              {landingPage.subtitle}
            </p>
          )}

          {landingPage.description && (
            <p
              className="text-muted-foreground max-w-xl mx-auto"
              data-testid="landing-description"
            >
              {landingPage.description}
            </p>
          )}
        </div>

        {landingPage.showSocialProof && landingPage.socialProofText && (
          <div
            className="flex items-center justify-center gap-2 mb-8 text-sm text-muted-foreground"
            data-testid="social-proof"
          >
            <Users className="h-4 w-4" />
            <span>{landingPage.socialProofText}</span>
          </div>
        )}

        {landingPage.form && sortedFields.length > 0 && (
          <Card className="shadow-lg" data-testid="form-card">
            <CardContent className="pt-6">
              <form onSubmit={handleSubmit} className="space-y-4">
                {sortedFields.map((field) => (
                  <div key={field.id} className="space-y-2">
                    {field.type !== "CHECKBOX" && (
                      <Label htmlFor={field.id}>
                        {field.label}
                        {field.required && (
                          <span className="text-red-500 ml-1">*</span>
                        )}
                      </Label>
                    )}
                    {renderField(field)}
                  </div>
                ))}

                <Button
                  type="submit"
                  className="w-full"
                  size="lg"
                  disabled={isSubmitting}
                  style={{ backgroundColor: primaryColor }}
                  data-testid="button-submit"
                >
                  {isSubmitting ? (
                    <>
                      <Spinner className="h-4 w-4 mr-2" />
                      Enviando...
                    </>
                  ) : (
                    landingPage.form.submitButtonText || "Enviar"
                  )}
                </Button>
              </form>
            </CardContent>
          </Card>
        )}

        {landingPage.footerText && (
          <p
            className="text-center text-sm text-muted-foreground mt-8"
            data-testid="landing-footer"
          >
            {landingPage.footerText}
          </p>
        )}
      </div>
    </div>
  );
}
