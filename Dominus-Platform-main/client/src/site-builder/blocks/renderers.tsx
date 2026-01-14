import { BlockContent } from '@shared/schema';
import { getBlockByKey } from './index';
import { Button } from '@/components/ui/button';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Star, Zap, Shield, Heart, Check, Loader2, Send, Calendar } from 'lucide-react';
import { useState, useEffect } from 'react';

interface BlockRendererProps {
  block: BlockContent;
  isPreview?: boolean;
  siteContext?: {
    siteId?: string;
    workspaceSlug?: string;
    siteSlug?: string;
  };
}

const iconMap: Record<string, any> = {
  zap: Zap,
  shield: Shield,
  heart: Heart,
  star: Star,
  check: Check,
};

export function BlockRenderer({ block, isPreview = false, siteContext }: BlockRendererProps) {
  const definition = getBlockByKey(block.type);
  if (!definition) {
    return (
      <div className="p-4 border border-dashed border-red-300 bg-red-50 rounded-lg">
        <p className="text-sm text-red-600">Bloco não encontrado: {block.type}</p>
      </div>
    );
  }

  const props = { ...definition.defaultProps, ...block.props };

  switch (block.type) {
    case 'hero-simple':
      return <HeroSimple {...props} />;
    case 'hero-with-image':
      return <HeroWithImage {...props} />;
    case 'features-grid':
      return <FeaturesGrid {...props} />;
    case 'features-list':
      return <FeaturesList {...props} />;
    case 'testimonials-carousel':
      return <TestimonialsCarousel {...props} />;
    case 'testimonials-grid':
      return <TestimonialsGrid {...props} />;
    case 'pricing-table':
      return <PricingTable {...props} />;
    case 'cta-simple':
      return <CtaSimple {...props} />;
    case 'cta-with-form':
      return <CtaWithForm {...props} isPreview={isPreview} />;
    case 'faq-accordion':
      return <FaqAccordion {...props} />;
    case 'header-simple':
      return <HeaderSimple {...props} />;
    case 'footer-simple':
      return <FooterSimple {...props} />;
    case 'content-text':
      return <ContentText {...props} />;
    case 'content-image':
      return <ContentImage {...props} />;
    case 'gallery-grid':
      return <GalleryGrid {...props} />;
    case 'form-embed':
      return <FormEmbed {...props} isPreview={isPreview} />;
    case 'blog-recent':
      return <BlogRecent {...props} isPreview={isPreview} workspaceSlug={siteContext?.workspaceSlug} siteSlug={siteContext?.siteSlug} />;
    case 'spacer':
      return <Spacer {...props} />;
    case 'divider':
      return <Divider {...props} />;
    default:
      return (
        <div className="p-4 border border-dashed border-gray-300 bg-gray-50 rounded-lg">
          <p className="text-sm text-gray-600">Renderização não implementada: {block.type}</p>
        </div>
      );
  }
}

function HeroSimple({ title, subtitle, ctaText, ctaLink, backgroundColor, textColor, alignment }: any) {
  return (
    <section 
      className="py-20 px-6"
      style={{ backgroundColor, color: textColor }}
    >
      <div className={`max-w-4xl mx-auto text-${alignment}`}>
        <h1 className="text-4xl md:text-5xl font-bold mb-6">{title}</h1>
        <p className="text-xl opacity-90 mb-8">{subtitle}</p>
        {ctaText && (
          <Button size="lg" variant="secondary" asChild>
            <a href={ctaLink}>{ctaText}</a>
          </Button>
        )}
      </div>
    </section>
  );
}

function HeroWithImage({ title, subtitle, ctaText, ctaLink, imageUrl, imagePosition }: any) {
  const content = (
    <div className="flex-1 space-y-6">
      <h1 className="text-4xl md:text-5xl font-bold">{title}</h1>
      <p className="text-xl text-muted-foreground">{subtitle}</p>
      {ctaText && (
        <Button size="lg" asChild>
          <a href={ctaLink}>{ctaText}</a>
        </Button>
      )}
    </div>
  );

  const image = (
    <div className="flex-1">
      <img 
        src={imageUrl} 
        alt={title}
        className="rounded-lg shadow-lg w-full object-cover"
      />
    </div>
  );

  return (
    <section className="py-20 px-6">
      <div className={`max-w-6xl mx-auto flex flex-col md:flex-row gap-12 items-center ${imagePosition === 'left' ? 'md:flex-row-reverse' : ''}`}>
        {content}
        {image}
      </div>
    </section>
  );
}

function FeaturesGrid({ title, subtitle, features, columns }: any) {
  const gridCols = columns === 4 ? 'md:grid-cols-4' : columns === 3 ? 'md:grid-cols-3' : 'md:grid-cols-2';
  
  return (
    <section className="py-20 px-6 bg-muted/30">
      <div className="max-w-6xl mx-auto">
        <div className="text-center mb-12">
          <h2 className="text-3xl font-bold mb-4">{title}</h2>
          <p className="text-lg text-muted-foreground">{subtitle}</p>
        </div>
        <div className={`grid ${gridCols} gap-8`}>
          {features?.map((feature: any, i: number) => {
            const Icon = iconMap[feature.icon] || Star;
            return (
              <div key={i} className="text-center p-6 bg-background rounded-xl shadow-sm">
                <div className="w-12 h-12 bg-primary/10 rounded-full flex items-center justify-center mx-auto mb-4">
                  <Icon className="size-6 text-primary" />
                </div>
                <h3 className="font-semibold mb-2">{feature.title}</h3>
                <p className="text-sm text-muted-foreground">{feature.description}</p>
              </div>
            );
          })}
        </div>
      </div>
    </section>
  );
}

function FeaturesList({ title, features }: any) {
  return (
    <section className="py-20 px-6">
      <div className="max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>
        <div className="space-y-6">
          {features?.map((feature: any, i: number) => (
            <div key={i} className="flex gap-4 items-start">
              <div className="w-8 h-8 bg-primary/10 rounded-full flex items-center justify-center flex-shrink-0">
                <Check className="size-4 text-primary" />
              </div>
              <div>
                <h3 className="font-semibold">{feature.title}</h3>
                <p className="text-muted-foreground">{feature.description}</p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function TestimonialsCarousel({ title, testimonials }: any) {
  return (
    <section className="py-20 px-6 bg-muted/30">
      <div className="max-w-4xl mx-auto">
        <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>
        <div className="bg-background p-8 rounded-xl shadow-sm">
          {testimonials?.[0] && (
            <div className="text-center">
              <p className="text-lg italic mb-6">"{testimonials[0].quote}"</p>
              <p className="font-semibold">{testimonials[0].name}</p>
              <p className="text-sm text-muted-foreground">{testimonials[0].role} - {testimonials[0].company}</p>
            </div>
          )}
        </div>
      </div>
    </section>
  );
}

function TestimonialsGrid({ title, testimonials, columns }: any) {
  const gridCols = columns === 3 ? 'md:grid-cols-3' : 'md:grid-cols-2';
  
  return (
    <section className="py-20 px-6">
      <div className="max-w-6xl mx-auto">
        <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>
        <div className={`grid ${gridCols} gap-6`}>
          {testimonials?.map((t: any, i: number) => (
            <div key={i} className="bg-muted/30 p-6 rounded-xl">
              <div className="flex gap-1 mb-4">
                {Array.from({ length: t.rating || 5 }).map((_, j) => (
                  <Star key={j} className="size-4 fill-yellow-400 text-yellow-400" />
                ))}
              </div>
              <p className="italic mb-4">"{t.quote}"</p>
              <p className="font-semibold">{t.name}</p>
              <p className="text-sm text-muted-foreground">{t.role}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function PricingTable({ title, subtitle, plans }: any) {
  return (
    <section className="py-20 px-6">
      <div className="max-w-6xl mx-auto">
        <div className="text-center mb-12">
          <h2 className="text-3xl font-bold mb-4">{title}</h2>
          <p className="text-lg text-muted-foreground">{subtitle}</p>
        </div>
        <div className="grid md:grid-cols-3 gap-8">
          {plans?.map((plan: any, i: number) => (
            <div 
              key={i} 
              className={`p-8 rounded-xl border-2 ${plan.highlighted ? 'border-primary bg-primary/5' : 'border-border'}`}
            >
              <h3 className="text-xl font-bold mb-2">{plan.name}</h3>
              <div className="mb-6">
                <span className="text-4xl font-bold">{plan.price}</span>
                <span className="text-muted-foreground">{plan.period}</span>
              </div>
              <ul className="space-y-3 mb-8">
                {plan.features?.map((f: string, j: number) => (
                  <li key={j} className="flex items-center gap-2">
                    <Check className="size-4 text-primary" />
                    <span>{f}</span>
                  </li>
                ))}
              </ul>
              <Button className="w-full" variant={plan.highlighted ? 'default' : 'outline'}>
                Escolher Plano
              </Button>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function CtaSimple({ title, subtitle, ctaText, ctaLink, backgroundColor, textColor }: any) {
  return (
    <section 
      className="py-16 px-6"
      style={{ backgroundColor, color: textColor }}
    >
      <div className="max-w-4xl mx-auto text-center">
        <h2 className="text-3xl font-bold mb-4">{title}</h2>
        <p className="text-lg opacity-90 mb-8">{subtitle}</p>
        <Button size="lg" variant="secondary" asChild>
          <a href={ctaLink}>{ctaText}</a>
        </Button>
      </div>
    </section>
  );
}

function CtaWithForm({ title, subtitle, formId, backgroundColor, textColor, isPreview }: any) {
  return (
    <section 
      className="py-16 px-6"
      style={{ backgroundColor, color: textColor }}
    >
      <div className="max-w-4xl mx-auto">
        <div className="text-center mb-8">
          <h2 className="text-3xl font-bold mb-4">{title}</h2>
          <p className="text-lg opacity-90">{subtitle}</p>
        </div>
        {formId ? (
          <div className="bg-white/10 backdrop-blur p-6 rounded-xl max-w-md mx-auto">
            <p className="text-center opacity-75">[Formulário ID: {formId}]</p>
          </div>
        ) : (
          <div className="bg-white/10 backdrop-blur p-6 rounded-xl max-w-md mx-auto text-center">
            <p className="opacity-75">Selecione um formulário</p>
          </div>
        )}
      </div>
    </section>
  );
}

function FaqAccordion({ title, items }: any) {
  return (
    <section className="py-20 px-6">
      <div className="max-w-3xl mx-auto">
        <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>
        <Accordion type="single" collapsible className="space-y-2">
          {items?.map((item: any, i: number) => (
            <AccordionItem key={i} value={`item-${i}`} className="border rounded-lg px-4">
              <AccordionTrigger className="text-left">{item.question}</AccordionTrigger>
              <AccordionContent>{item.answer}</AccordionContent>
            </AccordionItem>
          ))}
        </Accordion>
      </div>
    </section>
  );
}

function HeaderSimple({ logoText, logoUrl, links, ctaText, ctaLink }: any) {
  return (
    <header className="py-4 px-6 border-b">
      <div className="max-w-6xl mx-auto flex items-center justify-between">
        <div className="font-bold text-xl">
          {logoUrl ? <img src={logoUrl} alt={logoText} className="h-8" /> : logoText}
        </div>
        <nav className="hidden md:flex items-center gap-6">
          {links?.map((link: any, i: number) => (
            <a key={i} href={link.href} className="text-muted-foreground hover:text-foreground transition">
              {link.label}
            </a>
          ))}
          {ctaText && (
            <Button asChild size="sm">
              <a href={ctaLink}>{ctaText}</a>
            </Button>
          )}
        </nav>
      </div>
    </header>
  );
}

function FooterSimple({ companyName, links, socialLinks }: any) {
  return (
    <footer className="py-8 px-6 border-t bg-muted/30">
      <div className="max-w-6xl mx-auto flex flex-col md:flex-row items-center justify-between gap-4">
        <p className="text-sm text-muted-foreground">
          &copy; {new Date().getFullYear()} {companyName}. Todos os direitos reservados.
        </p>
        <div className="flex gap-4">
          {links?.map((link: any, i: number) => (
            <a key={i} href={link.href} className="text-sm text-muted-foreground hover:text-foreground transition">
              {link.label}
            </a>
          ))}
        </div>
      </div>
    </footer>
  );
}

function ContentText({ title, content, alignment }: any) {
  return (
    <section className="py-12 px-6">
      <div className={`max-w-4xl mx-auto text-${alignment}`}>
        {title && <h2 className="text-2xl font-bold mb-6">{title}</h2>}
        <div className="prose prose-lg max-w-none" dangerouslySetInnerHTML={{ __html: content }} />
      </div>
    </section>
  );
}

function ContentImage({ imageUrl, alt, caption, fullWidth }: any) {
  return (
    <section className={`py-8 ${fullWidth ? '' : 'px-6'}`}>
      <div className={fullWidth ? '' : 'max-w-4xl mx-auto'}>
        <img src={imageUrl} alt={alt} className="w-full rounded-lg" />
        {caption && <p className="text-center text-sm text-muted-foreground mt-4">{caption}</p>}
      </div>
    </section>
  );
}

function GalleryGrid({ title, images, columns }: any) {
  const gridCols = columns === 4 ? 'md:grid-cols-4' : columns === 3 ? 'md:grid-cols-3' : 'md:grid-cols-2';
  
  return (
    <section className="py-16 px-6">
      <div className="max-w-6xl mx-auto">
        {title && <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>}
        <div className={`grid ${gridCols} gap-4`}>
          {images?.map((img: any, i: number) => (
            <div key={i} className="aspect-square overflow-hidden rounded-lg">
              <img src={img.url} alt={img.alt} className="w-full h-full object-cover hover:scale-105 transition" />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function FormEmbed({ title, subtitle, formId, isPreview }: any) {
  const [form, setForm] = useState<any>(null);
  const [fields, setFields] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [submitted, setSubmitted] = useState(false);
  const [formData, setFormData] = useState<Record<string, string>>({});
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (formId && !isPreview) {
      setLoading(true);
      fetch(`/api/public/forms/${formId}`)
        .then(res => res.ok ? res.json() : Promise.reject('Form not found'))
        .then(data => {
          setForm(data.form);
          setFields(data.fields || []);
        })
        .catch(() => setError('Formulário não encontrado'))
        .finally(() => setLoading(false));
    }
  }, [formId, isPreview]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formId || isPreview) return;
    
    setSubmitting(true);
    try {
      const res = await fetch(`/api/public/forms/${formId}/submit`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
      });
      if (res.ok) {
        setSubmitted(true);
      } else {
        setError('Erro ao enviar formulário');
      }
    } catch {
      setError('Erro ao enviar formulário');
    } finally {
      setSubmitting(false);
    }
  };

  const renderField = (field: any) => {
    const value = formData[field.fieldKey] || '';
    const onChange = (val: string) => setFormData({ ...formData, [field.fieldKey]: val });

    switch (field.fieldType) {
      case 'EMAIL':
        return <Input type="email" placeholder={field.placeholder} value={value} onChange={(e) => onChange(e.target.value)} required={field.isRequired} />;
      case 'PHONE':
        return <Input type="tel" placeholder={field.placeholder} value={value} onChange={(e) => onChange(e.target.value)} required={field.isRequired} />;
      case 'TEXTAREA':
        return <Textarea placeholder={field.placeholder} value={value} onChange={(e) => onChange(e.target.value)} required={field.isRequired} rows={3} />;
      default:
        return <Input type="text" placeholder={field.placeholder} value={value} onChange={(e) => onChange(e.target.value)} required={field.isRequired} />;
    }
  };

  return (
    <section className="py-16 px-6 bg-muted/30">
      <div className="max-w-lg mx-auto">
        <div className="text-center mb-8">
          <h2 className="text-2xl font-bold mb-2">{title}</h2>
          <p className="text-muted-foreground">{subtitle}</p>
        </div>
        {!formId ? (
          <div className="bg-background p-6 rounded-xl shadow-sm border text-center">
            <p className="text-muted-foreground">Selecione um formulário nas configurações do bloco</p>
          </div>
        ) : isPreview ? (
          <div className="bg-background p-6 rounded-xl shadow-sm border text-center">
            <p className="text-muted-foreground">Formulário será exibido na versão publicada</p>
          </div>
        ) : loading ? (
          <div className="bg-background p-6 rounded-xl shadow-sm border flex items-center justify-center">
            <Loader2 className="size-6 animate-spin text-muted-foreground" />
          </div>
        ) : error ? (
          <div className="bg-background p-6 rounded-xl shadow-sm border text-center">
            <p className="text-red-500">{error}</p>
          </div>
        ) : submitted ? (
          <div className="bg-background p-6 rounded-xl shadow-sm border text-center">
            <Check className="size-12 text-green-500 mx-auto mb-4" />
            <p className="text-lg font-medium">{form?.successMessage || 'Enviado com sucesso!'}</p>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="bg-background p-6 rounded-xl shadow-sm border space-y-4">
            {fields.sort((a, b) => a.order - b.order).map((field) => (
              <div key={field.id} className="space-y-2">
                <Label>{field.label}{field.isRequired && <span className="text-red-500 ml-1">*</span>}</Label>
                {renderField(field)}
              </div>
            ))}
            <Button type="submit" className="w-full" disabled={submitting}>
              {submitting ? <Loader2 className="size-4 animate-spin mr-2" /> : <Send className="size-4 mr-2" />}
              {form?.submitButtonText || 'Enviar'}
            </Button>
          </form>
        )}
      </div>
    </section>
  );
}

function BlogRecent({ title, limit, showExcerpt, showDate, isPreview, siteId, workspaceSlug, siteSlug }: any) {
  const [posts, setPosts] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!isPreview && workspaceSlug && siteSlug) {
      setLoading(true);
      fetch(`/api/public/site/${workspaceSlug}/${siteSlug}/blog`)
        .then(res => res.ok ? res.json() : Promise.reject('Error'))
        .then(data => setPosts((data.posts || []).slice(0, limit || 3)))
        .catch(() => setPosts([]))
        .finally(() => setLoading(false));
    }
  }, [isPreview, workspaceSlug, siteSlug, limit]);

  const formatDate = (dateStr: string) => {
    try {
      return new Date(dateStr).toLocaleDateString('pt-BR');
    } catch {
      return '';
    }
  };

  if (isPreview) {
    return (
      <section className="py-16 px-6">
        <div className="max-w-6xl mx-auto">
          <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>
          <div className="grid md:grid-cols-3 gap-8">
            {Array.from({ length: limit || 3 }).map((_, i) => (
              <article key={i} className="bg-muted/30 rounded-xl overflow-hidden">
                <div className="aspect-video bg-muted" />
                <div className="p-6">
                  {showDate && <p className="text-sm text-muted-foreground mb-2">01/01/2025</p>}
                  <h3 className="font-semibold mb-2">Título do Post {i + 1}</h3>
                  {showExcerpt && <p className="text-sm text-muted-foreground">Resumo do post aqui...</p>}
                </div>
              </article>
            ))}
          </div>
        </div>
      </section>
    );
  }

  return (
    <section className="py-16 px-6">
      <div className="max-w-6xl mx-auto">
        <h2 className="text-3xl font-bold mb-12 text-center">{title}</h2>
        {loading ? (
          <div className="flex items-center justify-center py-12">
            <Loader2 className="size-8 animate-spin text-muted-foreground" />
          </div>
        ) : posts.length === 0 ? (
          <div className="text-center py-12 text-muted-foreground">
            <Calendar className="size-12 mx-auto mb-4 opacity-50" />
            <p>Nenhum post publicado ainda</p>
          </div>
        ) : (
          <div className="grid md:grid-cols-3 gap-8">
            {posts.map((post) => (
              <a 
                key={post.id} 
                href={`/site/${workspaceSlug}/${siteSlug}/blog/${post.slug}`}
                className="bg-muted/30 rounded-xl overflow-hidden hover:shadow-lg transition-shadow group"
              >
                <div className="aspect-video bg-muted overflow-hidden">
                  {post.featuredImage ? (
                    <img src={post.featuredImage} alt={post.title} className="w-full h-full object-cover group-hover:scale-105 transition-transform" />
                  ) : (
                    <div className="w-full h-full bg-gradient-to-br from-primary/10 to-primary/5" />
                  )}
                </div>
                <div className="p-6">
                  {showDate && post.publishedAt && (
                    <p className="text-sm text-muted-foreground mb-2">{formatDate(post.publishedAt)}</p>
                  )}
                  <h3 className="font-semibold mb-2 group-hover:text-primary transition-colors">{post.title}</h3>
                  {showExcerpt && post.excerpt && (
                    <p className="text-sm text-muted-foreground line-clamp-2">{post.excerpt}</p>
                  )}
                </div>
              </a>
            ))}
          </div>
        )}
      </div>
    </section>
  );
}

function Spacer({ height }: any) {
  return <div style={{ height: `${height}px` }} />;
}

function Divider({ style, color, width }: any) {
  return (
    <div className="px-6">
      <hr 
        style={{ 
          borderStyle: style, 
          borderColor: color, 
          width,
          margin: '2rem auto'
        }} 
      />
    </div>
  );
}
