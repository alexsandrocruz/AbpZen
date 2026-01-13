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
  import { useCreateFabConfig, useUpdateFabConfig } from "@/lib/abp/hooks/useFabConfigs";
import { toast } from "sonner";

const formSchema = z.object({
  
    idConfig: z.any(),
  
    imagemLogin: z.any(),
  
    imagemLoginCentral: z.any(),
  
    imagemLoginTickets: z.any(),
  
    tsAlteracao: z.any(),
  
    precoCombustivel: z.any(),
  
    dataBloqueioFinanceiro: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabConfigFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabConfigForm({
  isOpen,
  onClose,
  initialValues,
}: FabConfigFormProps) {
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

  const createMutation = useCreateFabConfig();
const updateMutation = useUpdateFabConfig();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabConfig updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabConfig created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabconfig:", error);
    toast.error(error.message || "Failed to save fabconfig");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabConfig": "Create fabConfig" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabconfig." : "Fill in the details to create a new fabconfig." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idConfig" > idConfig</Label>

<Input id="idConfig" type = "number" step = "any" {...register("idConfig") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="imagemLogin" > imagemLogin</Label>

<Input id="imagemLogin" {...register("imagemLogin") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="imagemLoginCentral" > imagemLoginCentral</Label>

<Input id="imagemLoginCentral" {...register("imagemLoginCentral") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="imagemLoginTickets" > imagemLoginTickets</Label>

<Input id="imagemLoginTickets" {...register("imagemLoginTickets") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="precoCombustivel" > precoCombustivel</Label>

<Input id="precoCombustivel" type = "number" step = "any" {...register("precoCombustivel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataBloqueioFinanceiro" > dataBloqueioFinanceiro</Label>

<Input id="dataBloqueioFinanceiro" {...register("dataBloqueioFinanceiro") } />

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
