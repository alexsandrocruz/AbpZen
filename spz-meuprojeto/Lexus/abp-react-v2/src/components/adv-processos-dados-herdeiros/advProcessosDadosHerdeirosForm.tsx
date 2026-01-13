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
  import { useCreateAdvProcessosDadosHerdeiros, useUpdateAdvProcessosDadosHerdeiros } from "@/lib/abp/hooks/useAdvProcessosDadosHerdeiroses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idHerdeiro: z.any(),
  
    idProcesso: z.any(),
  
    sequencia: z.any(),
  
    bancarioBancoId: z.any(),
  
    bancarioTipoConta: z.any(),
  
    bancarioAgencia: z.any(),
  
    bancarioConta: z.any(),
  
    bancarioFavorecido: z.any(),
  
    bancarioCpf: z.any(),
  
    bancarioPerc: z.any(),
  
    bancarioTarifa: z.any(),
  
    bancarioTarifaParcelas: z.any(),
  
    idHonorario: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProcessosDadosHerdeirosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProcessosDadosHerdeirosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProcessosDadosHerdeirosFormProps) {
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

  const createMutation = useCreateAdvProcessosDadosHerdeiros();
const updateMutation = useUpdateAdvProcessosDadosHerdeiros();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProcessosDadosHerdeiros updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProcessosDadosHerdeiros created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprocessosdadosherdeiros:", error);
    toast.error(error.message || "Failed to save advprocessosdadosherdeiros");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProcessosDadosHerdeiros": "Create advProcessosDadosHerdeiros" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprocessosdadosherdeiros." : "Fill in the details to create a new advprocessosdadosherdeiros." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idHerdeiro" > idHerdeiro</Label>

<Input id="idHerdeiro" type = "number" step = "any" {...register("idHerdeiro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso * </Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sequencia" > sequencia</Label>

<Input id="sequencia" type = "number" step = "any" {...register("sequencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioBancoId" > bancarioBancoId</Label>

<Input id="bancarioBancoId" type = "number" step = "any" {...register("bancarioBancoId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioTipoConta" > bancarioTipoConta</Label>

<Input id="bancarioTipoConta" {...register("bancarioTipoConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioAgencia" > bancarioAgencia</Label>

<Input id="bancarioAgencia" {...register("bancarioAgencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioConta" > bancarioConta</Label>

<Input id="bancarioConta" {...register("bancarioConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioFavorecido" > bancarioFavorecido</Label>

<Input id="bancarioFavorecido" {...register("bancarioFavorecido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioCpf" > bancarioCpf</Label>

<Input id="bancarioCpf" {...register("bancarioCpf") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioPerc" > bancarioPerc</Label>

<Input id="bancarioPerc" type = "number" step = "any" {...register("bancarioPerc") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioTarifa" > bancarioTarifa</Label>

<Input id="bancarioTarifa" type = "number" step = "any" {...register("bancarioTarifa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioTarifaParcelas" > bancarioTarifaParcelas</Label>

<Input id="bancarioTarifaParcelas" {...register("bancarioTarifaParcelas") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idHonorario" > idHonorario</Label>

<Input id="idHonorario" type = "number" step = "any" {...register("idHonorario") } />

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
