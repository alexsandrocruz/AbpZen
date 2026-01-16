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
  import { useCreateFinRecibos, useUpdateFinRecibos } from "@/lib/abp/hooks/useFinReciboses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idRecibo: z.any(),
  
    idLancamento: z.any(),
  
    numero: z.any(),
  
    referente: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinRecibosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinRecibosForm({
  isOpen,
  onClose,
  initialValues,
}: FinRecibosFormProps) {
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

  const createMutation = useCreateFinRecibos();
const updateMutation = useUpdateFinRecibos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finRecibos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finRecibos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finrecibos:", error);
    toast.error(error.message || "Failed to save finrecibos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finRecibos": "Create finRecibos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finrecibos." : "Fill in the details to create a new finrecibos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idRecibo" > idRecibo</Label>

<Input id="idRecibo" type = "number" step = "any" {...register("idRecibo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idLancamento" > idLancamento</Label>

<Input id="idLancamento" type = "number" step = "any" {...register("idLancamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="numero" > numero</Label>

<Input id="numero" type = "number" step = "any" {...register("numero") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="referente" > referente</Label>

<Input id="referente" {...register("referente") } />

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
