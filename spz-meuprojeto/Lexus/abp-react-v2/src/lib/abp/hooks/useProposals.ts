import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProposalsInput {
  filter?: string;
  clientId?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export function useProposals(input: GetProposalsInput = {}) {
  const { filter, clientId, skipCount = 0, maxResultCount = 10 } = input;

  return useQuery({
    queryKey: ["proposals", filter, clientId, skipCount, maxResultCount],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal", {
        params: {
          filter,
          clientId,
          skipCount,
          maxResultCount,
        },
      });
      return response.data;
    },
  });
}

export function useProposal(id: string) {
  return useQuery({
    queryKey: ["proposal", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/proposal/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProposal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/proposal", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposals"] });
    },
  });
}

export function useUpdateProposal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/proposal/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["proposals"] });
      queryClient.invalidateQueries({ queryKey: ["proposal", data.id] });
    },
  });
}

export function useDeleteProposal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/proposal/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposals"] });
    },
  });
}

export function useAllProposals() {
  return useQuery({
    queryKey: ["proposals", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}


