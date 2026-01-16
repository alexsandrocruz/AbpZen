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
  import { useCreateFabFormasPagamento, useUpdateFabFormasPagamento } from "@/lib/abp/hooks/useFabFormasPagamentos";
import { toast } from "sonner";

const formSchema = z.object({
  
    idFormaPagamento: z.any(),
  
    titulo: z.any(),
  
    ordem: z.any(),
  
    padrao: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idCondicaoPagamento: z.any(),
  
    contasPagar: z.any(),
  
    compras: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabFormasPagamentoFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabFormasPagamentoForm({
  isOpen,
  onClose,
  initialValues,
}: FabFormasPagamentoFormProps) {
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

  const createMutation = useCreateFabFormasPagamento();
const updateMutation = useUpdateFabFormasPagamento();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabFormasPagamento updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabFormasPagamento created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabformaspagamento:", error);
    toast.error(error.message || "Failed to save fabformaspagamento");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabFormasPagamento": "Create fabFormasPagamento" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabformaspagamento." : "Fill in the details to create a new fabformaspagamento." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idFormaPagamento" > idFormaPagamento</Label>

<Input id="idFormaPagamento" type = "number" step = "any" {...register("idFormaPagamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ordem" > ordem</Label>

<Input id="ordem" type = "number" step = "any" {...register("ordem") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="padrao" > padrao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="padrao"
checked = { watch("padrao") }
onCheckedChange = {(checked) => setValue("padrao", !!checked)}
              />
  < label htmlFor = "padrao" className = "text-sm font-normal" >
    { watch("padrao") ?"Enabled": "Disabled" }
    </label>
    </div>

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
  <Label htmlFor="idCondicaoPagamento" > idCondicaoPagamento</Label>

<Input id="idCondicaoPagamento" type = "number" step = "any" {...register("idCondicaoPagamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="contasPagar" > contasPagar</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="contasPagar"
checked = { watch("contasPagar") }
onCheckedChange = {(checked) => setValue("contasPagar", !!checked)}
              />
  < label htmlFor = "contasPagar" className = "text-sm font-normal" >
    { watch("contasPagar") ?"Enabled": "Disabled" }
    </label>
    </div>

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
