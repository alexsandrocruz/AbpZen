import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateFabCondicoesPagamento, useUpdateFabCondicoesPagamento } from "@/lib/abp/hooks/useFabCondicoesPagamentos";
import { toast } from "sonner";

const formSchema = z.object({
  
    idCondicaoPagamento: z.any(),
  
    titulo: z.any(),
  
    parcelas: z.any(),
  
    p1: z.any(),
  
    d1: z.any(),
  
    p2: z.any(),
  
    d2: z.any(),
  
    p3: z.any(),
  
    d3: z.any(),
  
    p4: z.any(),
  
    d4: z.any(),
  
    p5: z.any(),
  
    d5: z.any(),
  
    p6: z.any(),
  
    d6: z.any(),
  
    p7: z.any(),
  
    d7: z.any(),
  
    p8: z.any(),
  
    d8: z.any(),
  
    p9: z.any(),
  
    d9: z.any(),
  
    p10: z.any(),
  
    d10: z.any(),
  
    p11: z.any(),
  
    d11: z.any(),
  
    p12: z.any(),
  
    d12: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    compras: z.any(),
  
    vendas: z.any(),
  
    valorMinimo: z.any(),
  
    atendimento: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabCondicoesPagamentoFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabCondicoesPagamentoForm({
  isOpen,
  onClose,
  initialValues,
}: FabCondicoesPagamentoFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateFabCondicoesPagamento();
const updateMutation = useUpdateFabCondicoesPagamento();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabCondicoesPagamento updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabCondicoesPagamento created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabcondicoespagamento:", error);
    toast.error(error.message || "Failed to save fabcondicoespagamento");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabCondicoesPagamento": "Create fabCondicoesPagamento" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabcondicoespagamento." : "Fill in the details to create a new fabcondicoespagamento." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idCondicaoPagamento" > idCondicaoPagamento</Label>

<Input id="idCondicaoPagamento" type = "number" step = "any" {...register("idCondicaoPagamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="parcelas" > parcelas</Label>

<Input id="parcelas" type = "number" step = "any" {...register("parcelas") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p1" > p1</Label>

<Input id="p1" type = "number" step = "any" {...register("p1") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d1" > d1</Label>

<Input id="d1" type = "number" step = "any" {...register("d1") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p2" > p2</Label>

<Input id="p2" type = "number" step = "any" {...register("p2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d2" > d2</Label>

<Input id="d2" type = "number" step = "any" {...register("d2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p3" > p3</Label>

<Input id="p3" type = "number" step = "any" {...register("p3") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d3" > d3</Label>

<Input id="d3" type = "number" step = "any" {...register("d3") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p4" > p4</Label>

<Input id="p4" type = "number" step = "any" {...register("p4") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d4" > d4</Label>

<Input id="d4" type = "number" step = "any" {...register("d4") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p5" > p5</Label>

<Input id="p5" type = "number" step = "any" {...register("p5") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d5" > d5</Label>

<Input id="d5" type = "number" step = "any" {...register("d5") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p6" > p6</Label>

<Input id="p6" type = "number" step = "any" {...register("p6") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d6" > d6</Label>

<Input id="d6" type = "number" step = "any" {...register("d6") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p7" > p7</Label>

<Input id="p7" type = "number" step = "any" {...register("p7") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d7" > d7</Label>

<Input id="d7" type = "number" step = "any" {...register("d7") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p8" > p8</Label>

<Input id="p8" type = "number" step = "any" {...register("p8") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d8" > d8</Label>

<Input id="d8" type = "number" step = "any" {...register("d8") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p9" > p9</Label>

<Input id="p9" type = "number" step = "any" {...register("p9") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d9" > d9</Label>

<Input id="d9" type = "number" step = "any" {...register("d9") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p10" > p10</Label>

<Input id="p10" type = "number" step = "any" {...register("p10") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d10" > d10</Label>

<Input id="d10" type = "number" step = "any" {...register("d10") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p11" > p11</Label>

<Input id="p11" type = "number" step = "any" {...register("p11") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d11" > d11</Label>

<Input id="d11" type = "number" step = "any" {...register("d11") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="p12" > p12</Label>

<Input id="p12" type = "number" step = "any" {...register("p12") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="d12" > d12</Label>

<Input id="d12" type = "number" step = "any" {...register("d12") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="compras" > compras</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="compras"
checked = { watch("compras") }
onCheckedChange = {(checked) => setValue("compras", !!checked)}
              />
  < label htmlFor = "compras" className = "text-sm font-normal" >
    { watch("compras") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="vendas" > vendas</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="vendas"
checked = { watch("vendas") }
onCheckedChange = {(checked) => setValue("vendas", !!checked)}
              />
  < label htmlFor = "vendas" className = "text-sm font-normal" >
    { watch("vendas") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="valorMinimo" > valorMinimo</Label>

<Input id="valorMinimo" type = "number" step = "any" {...register("valorMinimo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="atendimento" > atendimento</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="atendimento"
checked = { watch("atendimento") }
onCheckedChange = {(checked) => setValue("atendimento", !!checked)}
              />
  < label htmlFor = "atendimento" className = "text-sm font-normal" >
    { watch("atendimento") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
